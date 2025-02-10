using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using User.Dtos;

namespace User.Features.GetUserById
{
    public class GetUserByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/user/{id}",
                async (Int64 id, ISender sender) =>
                {
                    var query = new GetUserByIdQuery(id);
                    var result = await sender.Send(query);

                    return result is not null ? Results.Ok(result) : Results.NotFound();
                })
                .Produces<UserDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}