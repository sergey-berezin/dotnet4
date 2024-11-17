using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/hello", (string name) => $"Hello, {name}!")
    .WithName("SingleHello")
    .WithOpenApi();

app.MapPost("/hello", ([FromBody] PostHelloInput input) =>
{
    return new PostHelloOutputs(
        input.Names.Length,
        input.Names.Select(name => $"Hello, {name}!").ToArray()
    );
}).WithName("ManyHellos").WithOpenApi();

app.Run();

record PostHelloInput(string[] Names);

record PostHelloOutputs(int Count, string[] Нellos);

