// ASP.NET Core + CQRS + Custom Mediator 예제: 학생 수강 신청 시스템

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Mediator
{
    // ===== 확장 메서드 =====
    public static class MediatorServiceExtensions
    {
        public static IServiceCollection AddCustomMediator(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddScoped<ICustomMediator, CustomMediator>();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var iface in type.GetInterfaces())
                    {
                        if (!iface.IsGenericType) continue;

                        var def = iface.GetGenericTypeDefinition();
                        if (def == typeof(ICustomRequestHandler<,>) || def == typeof(ICustomNotificationHandler<>))
                            services.AddScoped(iface, type);

                        if (def == typeof(ICustomPipelineBehavior<,>))
                            services.AddScoped(typeof(ICustomPipelineBehavior<,>), type);
                    }
                }
            }

            return services;
        }

        //public static void UseStudentEnrollmentEndpoints(this IApplicationBuilder app)
        //{
        //    app.UseRouting();
        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapPost("/enroll", async context =>
        //        {
        //            var mediator = context.RequestServices.GetRequiredService<IMediator>();
        //            var form = await context.Request.ReadFormAsync();
        //            var student = form["student"];
        //            var course = form["course"];

        //            var result = await mediator.Send(new EnrollStudentCommand { StudentName = student!, CourseName = course! });
        //            await mediator.Publish(new EnrollmentCreatedNotification { Message = result });
        //            await context.Response.WriteAsync(result);
        //        });

        //        endpoints.MapGet("/enrollment", async context =>
        //        {
        //            var mediator = context.RequestServices.GetRequiredService<IMediator>();
        //            var student = context.Request.Query["student"];
        //            var result = await mediator.Send(new GetEnrollmentQuery { StudentName = student! });
        //            await context.Response.WriteAsync(result);
        //        });
        //    });
        //}
    }

    // ===== Core 인터페이스 및 구현체 =====
    public interface ICustomRequest<TResponse> { }
    public interface ICustomNotification { }

    public interface ICustomCommand<TResponse> : ICustomRequest<TResponse> { }
    public interface ICustomQuery<TResponse> : ICustomRequest<TResponse> { }

    public interface ICustomRequestHandler<TRequest, TResponse> where TRequest : ICustomRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public interface ICustomNotificationHandler<TNotification> where TNotification : ICustomNotification
    {
        Task Handle(TNotification notification, CancellationToken cancellationToken);
    }

    public interface ICustomPipelineBehavior<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next);
    }

    public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

    public interface ICustomMediator
    {
        Task<TResponse> Send<TResponse>(ICustomRequest<TResponse> request, CancellationToken cancellationToken = default);
        Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default, bool parallel = true) where TNotification : ICustomNotification;
    }

    public class CustomMediator : ICustomMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private static readonly ConcurrentDictionary<Type, Func<object, object, CancellationToken, Task>> _handlerInvokerCache = new();

        public async Task<TResponse> Send<TResponse>(ICustomRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            try
            {
                var requestType = request.GetType();
                var handlerType = typeof(ICustomRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
                var handler = _serviceProvider.GetRequiredService(handlerType);
                var handlerImplType = handler.GetType();

                var invoker = _handlerInvokerCache.GetOrAdd(handlerImplType, static implType =>
                {
                    var handlerParam = Expression.Parameter(typeof(object), "handler");
                    var requestParam = Expression.Parameter(typeof(object), "request");
                    var ctParam = Expression.Parameter(typeof(CancellationToken), "cancellationToken");

                    var castHandler = Expression.Convert(handlerParam, implType);

                    var interfaceType = implType.GetInterfaces().FirstOrDefault(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICustomRequestHandler<,>));

                    if (interfaceType == null)
                        throw new InvalidOperationException($"'{implType.FullName}' 타입이 IRequestHandler<,> 를 구현하지 않았습니다.");

                    var requestGenericType = interfaceType.GetGenericArguments()[0];
                    var castRequest = Expression.Convert(requestParam, requestGenericType);

                    var handleMethod = interfaceType.GetMethod(nameof(ICustomRequestHandler<ICustomRequest<object>, object>.Handle));
                    var call = Expression.Call(castHandler, handleMethod, castRequest, ctParam);
                    var castToObjectTask = Expression.Convert(call, typeof(Task));

                    var lambda = Expression.Lambda<Func<object, object, CancellationToken, Task>>(castToObjectTask, handlerParam, requestParam, ctParam);

                    return lambda.Compile();
                });

                RequestHandlerDelegate<TResponse> handlerDelegate = async () =>
                {
                    var task = invoker(handler, request, cancellationToken);
                    await task.ConfigureAwait(false);
                    var resultProperty = task.GetType().GetProperty("Result");
                    return (TResponse?)resultProperty?.GetValue(task)!;
                };

                var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
                var behaviors = _serviceProvider.GetServices(behaviorType).Cast<object>().Reverse().ToList();

                foreach (var behavior in behaviors)
                {
                    var behaviorInterface = behavior.GetType().GetInterfaces()
                        .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICustomPipelineBehavior<,>));

                    var handleMethod = behaviorInterface.GetMethod(nameof(ICustomPipelineBehavior<ICustomRequest<object>, object>.Handle))!;

                    var next = handlerDelegate;
                    handlerDelegate = () => (Task<TResponse>)handleMethod.Invoke(behavior, new object[] { request, cancellationToken, next })!;
                }

                return await handlerDelegate();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default, bool parallel = true) where TNotification : ICustomNotification
        {
            ArgumentNullException.ThrowIfNull(notification);

            var handlerType = typeof(ICustomNotificationHandler<>).MakeGenericType(typeof(TNotification));
            var handlers = _serviceProvider.GetServices(handlerType).Cast<object>().ToList();

            if (parallel)
            {
                var tasks = handlers.Select(handler =>
                {
                    var method = handler.GetType().GetMethod("Handle")!;
                    return (Task)method.Invoke(handler, new object[] { notification, cancellationToken })!;
                });

                await Task.WhenAll(tasks);
            }
            else
            {
                foreach (var handler in handlers)
                {
                    var method = handler.GetType().GetMethod("Handle")!;
                    await (Task)method.Invoke(handler, new object[] { notification, cancellationToken })!;
                }
            }
        }
    }



    // ===== 도메인 =====
    public class EnrollStudentCommand : ICustomCommand<string>
{
    public string StudentName { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
}

//public class GetEnrollmentQuery : IQuery<string>
//{
//    public string StudentName { get; set; } = string.Empty;
//}

//public class EnrollmentCreatedNotification : INotification
//{
//    public string Message { get; set; } = string.Empty;
//}

//public class EnrollStudentHandler : IRequestHandler<EnrollStudentCommand, string>
//{
//    private static readonly Dictionary<string, string> Database = new();

//    public Task<string> Handle(EnrollStudentCommand request, CancellationToken cancellationToken)
//    {
//        Database[request.StudentName] = request.CourseName;
//        return Task.FromResult($"{request.StudentName} has enrolled in {request.CourseName}");
//    }
//}

//public class GetEnrollmentHandler : IRequestHandler<GetEnrollmentQuery, string>
//{
//    private static readonly Dictionary<string, string> Database = EnrollStudentHandler.Database;

//    public Task<string> Handle(GetEnrollmentQuery request, CancellationToken cancellationToken)
//    {
//        if (Database.TryGetValue(request.StudentName, out var course))
//            return Task.FromResult($"{request.StudentName} is enrolled in {course}");
//        return Task.FromResult($"{request.StudentName} is not enrolled in any course");
//    }
//}

//public class NotificationHandler : INotificationHandler<EnrollmentCreatedNotification>
//{
//    public Task Handle(EnrollmentCreatedNotification notification, CancellationToken cancellationToken)
//    {
//        Console.WriteLine("[EVENT] " + notification.Message);
//        return Task.CompletedTask;
//    }
//}

//public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//{
//    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
//    {
//        Console.WriteLine($"Handling {typeof(TRequest).Name}");
//        var result = await next();
//        Console.WriteLine($"Handled {typeof(TResponse).Name}");
//        return result;
//    }
//}

// ===== Entry Point =====
//public class Program
//{
//    public static void Main(string[] args)
//    {
//        Host.CreateDefaultBuilder(args)
//            .ConfigureWebHostDefaults(webBuilder =>
//            {
//                webBuilder.ConfigureServices(services =>
//                {
//                    services.AddCustomMediator(typeof(Program).Assembly);
//                });
//                webBuilder.Configure(app =>
//                {
//                    app.UseStudentEnrollmentEndpoints();
//                });
//            })
//            .Build()
//            .Run();
//    }
//}
}
