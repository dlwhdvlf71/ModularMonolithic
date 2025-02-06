using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Todo.Dtos;

namespace Todo.Features.GetTodo
{
    public class GetTodoEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/todo",
                async ([FromBody] TodoDto dto, ISender sender) =>
            {
                var command = new GetTodoCommand(dto);

                var result = await sender.Send(command);

                return Results.Ok(result);
            })
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Todo in Summary")
            .WithDescription("Create Todo in Description");
        }
    }
}