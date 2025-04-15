using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Mediator;

namespace Login.Features.LangcodeLogin
{
    public class LangcodeLoginEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("login/{userId}", async (string userId, [FromServices] ICustomMediator mediator) =>
            {
                var command = new LangcodeLoginCommand(userId);

                var result = await mediator.Send(command);

                return result is not null ? Results.Ok(result) : Results.NotFound();
            })
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("login summary")
            .WithDescription("login description");
        }
    }
}