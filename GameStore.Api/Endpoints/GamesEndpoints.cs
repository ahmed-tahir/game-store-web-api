using System;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameById = "GetGameById";

    // private static readonly List<GameDto> games = new()
    // {
    //     new GameDto(1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", 59.99m, new DateOnly(2017, 3, 3)),
    //     new GameDto(2, "God of War", "Action-adventure", 49.99m, new DateOnly(2018, 4, 20)),
    //     new GameDto(3, "Red Dead Redemption 2", "Action-adventure", 39.99m, new DateOnly(2018, 10, 26)),
    //     new GameDto(4, "The Witcher 3: Wild Hunt", "Action RPG", 29.99m, new DateOnly(2015, 5, 19)),
    //     new GameDto(5, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18))
    // };

    public static WebApplication MapGamesEndpoints(this WebApplication app)
    {
        //GET /games
        app.MapGet("games", async (GameStoreDbContext dbContext) =>
        {
            return await dbContext.Games
                                  .Include(g => g.Genre)
                                  .Select(g => g.ToDto())
                                  .AsNoTracking()
                                  .ToListAsync();
        });

        //GET /game/{id}
        app.MapGet("games/{id}", async (int id, GameStoreDbContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            return game is not null ? Results.Ok(game.ToGameDetailsDto()) : Results.NotFound();
        })
        .WithName(GetGameById);

        //POST /games
        app.MapPost("games", async (CreateGameDto newGame, GameStoreDbContext dbContext) =>
        {
            if (string.IsNullOrEmpty(newGame.Name))
            {
                return Results.BadRequest("Name is required");
            }

            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetGameById, new { id = game.Id }, game.ToGameDetailsDto());
        })
        .WithParameterValidation();

        //PUT /games/{id}
        app.MapPut("games/{id}", async (int id, UpdateGameDto updatedGame, GameStoreDbContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);

            if (game is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(game)
                     .CurrentValues
                     .SetValues(updatedGame.ToEntity(id));
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithParameterValidation();

        //DELETE /games/{id}
        app.MapDelete("games/{id}", async (int id, GameStoreDbContext dbContext) =>
        {
            await dbContext.Games
                           .Where(g => g.Id == id)
                           .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return app;
    }
}
