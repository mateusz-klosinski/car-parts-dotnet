using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SDA.MK.CarParts;
using SDA.MK.CarParts.Endpoints;
using SDA.MK.CarParts.Responses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>();

builder.Services.AddCors(options =>
{
	options.AddPolicy("Dev", builder => builder
		.AllowAnyHeader()
		.AllowAnyMethod()
		.AllowAnyOrigin()
	);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("Dev");

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

app.MapClientsEndpoints();
app.MapPartsEndpoints();
app.MapBasketEndpoints();

var dbContext = app.Services.GetRequiredService<Context>();

dbContext.Database.Migrate();

app.Run();