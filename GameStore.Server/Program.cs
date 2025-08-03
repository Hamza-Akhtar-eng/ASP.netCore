using GameStore.Server.Models;
List<Game> games = new()
    {
        new Game()
        {
            Id = 1,
            Name = "Street Fighter II",
            Genre = "Fighting",
            Price = 19.99M,
            ReleaseDate = new DateTime(1991, 2, 1)
        },
        new Game()
        {
            Id = 2,
            Name = "Final Fantasy XIV",
            Genre = "Roleplaying",
            Price = 59.99M,
            ReleaseDate =
            new DateTime(2010, 9, 30)
        },
        new Game()
        {
            Id = 3,
            Name = "FIFA 23",
            Genre = "Sports",
            Price = 69.99M,
            ReleaseDate = new DateTime(2022, 9, 27)
        }
    };
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
//Route Groups
var group = app.MapGroup("/games").WithParameterValidation();

//Get /games
group.MapGet("/", () => games);

//Get /games/{id}
group.MapGet("/{id}", (int id) =>
{
    Game? game = games.Find(game => game.Id == id);
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
group.MapPost("/", (Game game) =>
{
    game.Id = games.Max(game => game.Id) + 1;
    games.Add(game);

    return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
});

//Put /games/(id)
group.MapPut("/{id}", (int id, Game updatedGame) =>
{
    Game? existingGame = games.Find(game => game.Id == id);
    if (existingGame is null)
    {

        updatedGame.Id = id;
        games.Add(updatedGame);
        return Results.CreatedAtRoute("GetGame", new { id = updatedGame.Id }, updatedGame);
    }
    existingGame.Name = updatedGame.Name;
    existingGame.Price = updatedGame.Price;
    existingGame.Genre = updatedGame.Genre;
    existingGame.ReleaseDate = updatedGame.ReleaseDate;

    return Results.NoContent();
});

//Delete /games/{id}
group.MapDelete("/{id}", (int id) =>
{
    Game? existingGame = games.Find(game => game.Id == id);
    if (existingGame is null)
    {
        return Results.NotFound();
    }
    games.Remove(existingGame);

    return Results.NoContent();
});
app.Run();
