using GameStore.Server.Data;
using GameStore.Server.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.WithOrigins("http://localhost:5259").AllowAnyHeader().AllowAnyMethod();
}));

var connString = builder.Configuration.GetConnectionString("GameStoreContext");
builder.Services.AddSqlServer<GameStoreContext>(connString);

var app = builder.Build();
//use the cors policy for every new request 
app.UseCors();
//Route Groups
var group = app.MapGroup("/games").WithParameterValidation();

//Get /games
group.MapGet("/", async (string filter, GameStoreContext context) =>
{
    var games = context.Games.AsNoTracking();
    if (filter is not null)
    {
        games = games.Where(game => game.Name.Contains(filter) || game.Genre.Contains(filter));
    }
    return await games.ToListAsync();
});

//Get /games/{id}
group.MapGet("/{id}", async (int id, GameStoreContext context) =>
{
    Game? game =  await context.Games.FindAsync(id);
    if (game is null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok(game);
    }
})
.WithName("GetGame");

//Post /games
group.MapPost("/", async (Game game,GameStoreContext context) =>
{
    context.Games.Add(game);
    await context.SaveChangesAsync();
    return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
});

//Put /games/(id)
group.MapPut("/{id}", async (int id, Game updatedGame, GameStoreContext context) =>
{
    var rowsAffected = await context.Games.Where(game => game.Id == id).ExecuteUpdateAsync(updates =>

        updates.SetProperty(game => game.Name, updatedGame.Name).
        SetProperty(game => game.Genre, updatedGame.Genre).
        SetProperty(game => game.Price, updatedGame.Price).
        SetProperty(game => game.ReleaseDate, updatedGame.ReleaseDate));
    return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});

//Delete /games/{id}
group.MapDelete("/{id}", async(int id, GameStoreContext context) =>
{
    var rowsAffected = await context.Games.Where(game => game.Id == id).
    ExecuteDeleteAsync();

     return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});
app.Run();
