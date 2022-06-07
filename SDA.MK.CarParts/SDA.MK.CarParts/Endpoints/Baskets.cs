﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDA.MK.CarParts.Requests;
using SDA.MK.CarParts.Responses;

namespace SDA.MK.CarParts.Endpoints
{
	public static class Baskets
	{
		public static IEndpointRouteBuilder MapBasketEndpoints(this IEndpointRouteBuilder endpoints)
		{
			endpoints.MapGet("/basket", async ([FromServices] Context context, [FromQuery] Guid clientId) =>
			{
				var basket = await context.Baskets
					.Where(b => b.Client.Id == clientId)
					.Select(b => new BasketResponse(
						b.Id, 
						b.BasketEntries.Select(be => 
							new BasketEntryReponse(
								be.Id, 
								new PartResponse(be.Part.Id, be.Part.Name, be.Part.Price), 
								be.Amount, 
								be.Price
							)), 
						b.TotalItems, 
						b.Price
						))
					.FirstOrDefaultAsync();

				if (basket is null)
				{
					throw new ArgumentException($"Cannot find basket for client: {clientId}, first create correct client!");
				}

				return Results.Ok(basket);
			})
			.WithName("Get Basket for Client")
			.Produces<BasketResponse>(StatusCodes.Status200OK, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");


			endpoints.MapPost("/basket/part/", async ([FromServices] Context context, [FromBody] AddPartToBasketRequest request) =>
			{
				var basket = await context.Baskets
					.Where(b => b.Client.Id == request.ClientId)
					.FirstOrDefaultAsync();

				var part = await context.Parts
					.Where(p => p.Id == request.PartId)
					.FirstOrDefaultAsync();

				if (basket is null)
				{
					throw new ArgumentException($"Cannot find basket for client: {request.ClientId}, first create correct client!");
				}

				if (part is null)
				{
					throw new ArgumentException($"Cannot find part: {request.PartId}.");
				}

				basket.AddPart(part);
				await context.SaveChangesAsync();

				return Results.Ok(BasketResponse.FromBasket(basket));
			})
			.WithName("Add part to basket")
			.Produces<BasketResponse>(StatusCodes.Status200OK, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");


			endpoints.MapDelete("/basket/{clientId}/part/{partId}", async ([FromServices] Context context, [FromRoute] Guid clientId, [FromRoute] Guid partId) =>
			{
				var basket = await context.Baskets
					.Where(b => b.Client.Id == clientId)
					.FirstOrDefaultAsync();

				var part = await context.Parts
					.Where(p => p.Id == partId)
					.FirstOrDefaultAsync();

				if (basket is null)
				{
					throw new ArgumentException($"Cannot find basket for client: {clientId}, first create correct client!");
				}

				if (part is null)
				{
					throw new ArgumentException($"Cannot find part: {partId}.");
				}

				basket.RemovePart(part);
				await context.SaveChangesAsync();

				return Results.Ok(BasketResponse.FromBasket(basket));
			})
			.WithName("Remove part from basket")
			.Produces<BasketResponse>(StatusCodes.Status200OK, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");

			endpoints.MapPut("/basket/part", async ([FromServices] Context context, [FromBody] ChangePartAmount request) =>
			{
				var basket = await context.Baskets
					.Where(b => b.Client.Id == request.ClientId)
					.FirstOrDefaultAsync();

				if (basket is null)
				{
					throw new ArgumentException($"Cannot find basket for client: {request.ClientId}, first create correct client!");
				}

				basket.ChangeAmount(request.PartId, request.Amount);
				await context.SaveChangesAsync();

				return Results.Ok(BasketResponse.FromBasket(basket));
			})
			.WithName("Change part amount in basket")
			.Produces<BasketResponse>(StatusCodes.Status200OK, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
			.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");

			return endpoints;
		}
	}
}