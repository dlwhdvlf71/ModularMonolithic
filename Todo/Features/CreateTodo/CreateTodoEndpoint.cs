using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Todo.Dtos;

namespace Todo.Features.CreateTodo
{
    public class CreateTodoEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/todo",
                async ([FromBody] TodoDto dto, ISender sender) =>
                {
                    var command = new CreateTodoCommand(dto);

                    var result = await sender.Send(command);

                    return Results.Created($"/todo/{result.id}", result);
                    //return Results.Ok(result);
                })
            //.Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Todo in Summary")
            .WithDescription("Create Todo in Description");
        }
    }
}