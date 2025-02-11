using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace User.Features.LoginUser
{
    public class LoginUserEndpoint : ICarterModule
    {
        public record LoginUserRequest(string Email, string Password);

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/login",
                async ([FromBody] LoginUserRequest request, ISender sender) =>
                {
                    var result = await sender.Send(new LoginCommand(request.Email, request.Password));

                    return result is not null ? Results.Ok(result) : Results.NotFound();
                })
                .Produces<string>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}