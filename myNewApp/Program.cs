using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var todos = new List<Todo>
{
    new Todo(1, "Learn ASP.NET Core", DateTime.Now.AddDays(7), false),
    new Todo(2, "Build a web API", DateTime.Now.AddDays(14), false),
    new Todo(3, "Deploy to Azure", DateTime.Now.AddDays(21), false)
};

app.MapGet("/todos", () => todos);

app.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound> (int id) =>
{
    var targetTodo = todos.SingleOrDefault(t => t.Id == id);
    return targetTodo is not null
        ? TypedResults.Ok(targetTodo)
        : TypedResults.NotFound();
});

app.MapPost("/todos", (Todo task) =>
{
    todos.Add(task);
    return Results.Created($"/todos/{task.Id}", task);
});

app.MapDelete("/todos/{id}", (int id) =>
{
    var targetTodo = todos.SingleOrDefault(t => t.Id == id);
    if (targetTodo is null)
    {
        return Results.NotFound();
    }

    todos.Remove(targetTodo);
    return Results.NoContent();
});



app.Run();


public record Todo(int Id, string Name, DateTime DueDate, bool IsCompleted);