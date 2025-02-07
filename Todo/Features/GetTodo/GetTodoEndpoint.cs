using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Todo.Dtos;

namespace Todo.Features.GetTodo
{
    public class GetTodoEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/todo/{id}",
                async (Int64 id, ISender sender) =>
                {
                    var query = new GetTodoQuery(id);
                    var result = await sender.Send(query);
                    return result is not null ? Results.Ok(result) : Results.NotFound();
                })
                .Produces<TodoDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get todo by id in summary")
                .WithDescription("Get todo by id in description");
        }
    }
}