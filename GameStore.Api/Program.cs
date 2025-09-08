using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

const string GetGameById = "GetGameById";

List<GameDto> games = new()
{
    new GameDto(1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", 59.99m, new DateOnly(2017, 3, 3)),
    new GameDto(2, "God of War", "Action-adventure", 49.99m, new DateOnly(2018, 4, 20)),
    new GameDto(3, "Red Dead Redemption 2", "Action-adventure", 39.99m, new DateOnly(2018, 10, 26)),
    new GameDto(4, "The Witcher 3: Wild Hunt", "Action RPG", 29.99m, new DateOnly(2015, 5, 19)),
    new GameDto(5, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18))
};

//GET /games
app.MapGet("games", () => games);

//GET /game/{id}
app.MapGet("games/{id}", (int id) =>
{
    var game = games.Find(g => g.Id == id);
    return game is not null ? Results.Ok(game) : Results.NotFound();
})
.WithName(GetGameById);

//POST /games
app.MapPost("games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);
    games.Add(game);
    return Results.CreatedAtRoute(GetGameById, new { id = game.Id }, game);
});

//PUT /games/{id}
app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) =>
{
    var index = games.FindIndex(g => g.Id == id);

    if( index == -1)
    {
        return Results.NotFound();
    }

    games[index] = new GameDto(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate); 
    return Results.NoContent();
});

//DELETE /games/{id}
app.MapDelete("games/{id}", (int id) =>
{
    games.RemoveAll(g => g.Id == id);
    return Results.NoContent();
});

app.MapGet("/", () => "Hello World!");

app.Run();
