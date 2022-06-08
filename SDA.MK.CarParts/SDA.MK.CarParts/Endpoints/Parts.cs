using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDA.MK.CarParts.Models;
using SDA.MK.CarParts.Requests;
using SDA.MK.CarParts.Responses;

namespace SDA.MK.CarParts.Endpoints
{
	public static class Parts
	{
		public static IEndpointRouteBuilder MapPartsEndpoints(this IEndpointRouteBuilder endpoints)
		{
			endpoints.MapGet("/part", async ([FromServices] Context context) =>
			{
				var parts = await context.Parts
				.Select(p => new PartResponse(p.Id, p.Name, p.Price))
				.ToArrayAsync();

				return Results.Ok(parts);
			})
			.WithName("Get parts")
			.Produces<IEnumerable<PartResponse>>(StatusCodes.Status200OK, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");

			endpoints.MapGet("/part/{id}", async ([FromServices] Context context, [FromRoute] Guid id) =>
			{
				var parts = await context.Parts
				.Where(p => p.Id == id)
				.Select(p => new PartResponse(p.Id, p.Name, p.Price))
				.FirstOrDefaultAsync();

				return Results.Ok(parts);
			})
			.WithName("Get part")
			.Produces<PartResponse>(StatusCodes.Status200OK, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");

			endpoints.MapPost("/part", async ([FromServices] Context context, [FromBody] CreatePartRequest request) =>
			{
				var part = new Part(request.Name, request.Price);

				context.Parts.Add(part);
				await context.SaveChangesAsync();

				return Results.Ok(new PartResponse(part.Id, part.Name, part.Price));
			})
		    .WithName("Add part")
			.Produces<PartResponse>(StatusCodes.Status200OK, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");

			endpoints.MapPut("/part/{partId}", async ([FromServices] Context context, [FromRoute] Guid partId, [FromBody] UpdatePartRequest request) =>
			{
				var part = await context.Parts.FirstOrDefaultAsync(p => p.Id == partId);

				if (part is null)
				{
					throw new ArgumentException($"Part with id {partId} does not exist!");
				}

				part.Update(request.Name, request.Price);
				await context.SaveChangesAsync();

				return Results.Ok(new PartResponse(part.Id, part.Name, part.Price));
			})
			.WithName("Update part")
			.Produces<PartResponse>(StatusCodes.Status200OK, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");

			endpoints.MapDelete("/part/{partId}", async ([FromServices] Context context, [FromRoute] Guid partId) =>
			{
				var part = await context.Parts.FirstOrDefaultAsync(p => p.Id == partId);

				if (part is null)
				{
					throw new ArgumentException($"Part with id {partId} does not exist!");
				}

				context.Remove(part);
				await context.SaveChangesAsync();

				return Results.Ok(part.Id);
			})
			.WithName("Delete part")
			.Produces<Guid>(StatusCodes.Status200OK, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");

			return endpoints;
		}
	}
}
