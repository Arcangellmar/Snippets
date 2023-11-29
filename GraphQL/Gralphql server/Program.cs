using Gralphql_server.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<QueryTest>()
    .AddMutationType<MutationTest>();

var app = builder.Build();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
});

app.MapGraphQL();

app.Run();