using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDA.MK.CarParts;
using SDA.MK.CarParts.Models;
using SDA.MK.CarParts.Requests;
using SDA.MK.CarParts.Responses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// Exceptions
app.UseExceptionHandler(handlerApp =>
{
	handlerApp.Run(async context =>
	{
		var exceptionHandlerPathFeature =
			 context.Features.Get<IExceptionHandlerPathFeature>();

		context.Response.ContentType = "application/json";

		var errorWithStatusCode = exceptionHandlerPathFeature?.Error switch
		{
			ArgumentException ex => (new ErrorResponse(ex.Message), StatusCodes.Status400BadRequest),
			_ => (new ErrorResponse("Unknown error occured"), StatusCodes.Status500InternalServerError)
		};

		context.Response.StatusCode = errorWithStatusCode.Item2;
		await context.Response.WriteAsJsonAsync(errorWithStatusCode.Item1);
	});
});

// Client
app.MapPost("/client", async ([FromServices] Context context) =>
{
	var client = new Client(Guid.NewGuid());

	context.Clients.Add(client);
	await context.SaveChangesAsync();

	return Results.Ok(new ClientCreatedResponse(client.Id));
})
.WithName("CreateClient")
.Produces<ClientCreatedResponse>(StatusCodes.Status200OK, "application/json")
.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");

// Parts
app.MapGet("/part", async ([FromServices] Context context) =>
{
	var parts = await context.Parts
	.Select(p => new PartResponse(p.Id, p.Name, p.Price))
	.ToArrayAsync();

	return Results.Ok(parts);
})
.WithName("GetParts")
.Produces<IEnumerable<PartResponse>>(StatusCodes.Status200OK, "application/json")
.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");

app.MapPost("/part", async ([FromServices] Context context, [FromBody] CreatePartRequest request) =>
{
	var part = new Part(request.Name, request.Price);

	context.Parts.Add(part);
	await context.SaveChangesAsync();

	return Results.Ok(new PartResponse(part.Id, part.Name, part.Price));
})
.Produces<PartResponse>(StatusCodes.Status200OK, "application/json")
.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");

app.MapPut("/part/{partId}", async ([FromServices] Context context, [FromRoute] Guid partId, [FromBody] UpdatePartRequest request) =>
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
.Produces<PartResponse>(StatusCodes.Status200OK, "application/json")
.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");

app.MapDelete("/part/{partId}", async ([FromServices] Context context, [FromRoute] Guid partId) =>
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
.Produces<Guid>(StatusCodes.Status200OK, "application/json")
.Produces<ErrorResponse>(StatusCodes.Status400BadRequest, "application/json")
.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, "application/json");

app.Run();