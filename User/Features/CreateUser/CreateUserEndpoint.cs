using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using User.Dtos;

namespace User.Features.CreateUser
{
    public class CreateUserEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/user",
                async ([FromBody] UserDto UserDto, ISender sender) =>
            {
                var command = new CreateUserCommand(UserDto);

                var result = await sender.Send(command);

                return Results.Created($"/user/{result.id}", result);
            })
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}