using Microsoft.AspNetCore.Mvc;
using SDA.MK.CarParts.Models;
using SDA.MK.CarParts.Responses;

namespace SDA.MK.CarParts.Endpoints
{
	public static class Clients
	{
		public static IEndpointRouteBuilder MapClientsEndpoints(this IEndpointRouteBuilder endpoints)
		{
			endpoints.MapPost("/client", async ([FromServices] Context context) =>
			{
				var client = new Client(Guid.NewGuid());
				var basket = new Basket(Guid.NewGuid(), client);

				context.Clients.Add(client);
				context.Baskets.Add(basket);

				await context.SaveChangesAsync();

				return Results.Ok(new ClientCreatedResponse(client.Id));
			})
			.WithName("Create client")
			.Produces<ClientCreatedResponse>(StatusCodes.Status200OK, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");

			return endpoints;
		}
	}
}
