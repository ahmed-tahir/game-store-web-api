using System;
using GameStore.Api.Data;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("genres");

        group.MapGet("/", async (GameStoreDbContext dbContext) =>
        {
            var genres = await dbContext.Genres
                                        .Select(g => g.ToDto())
                                        .AsNoTracking()
                                        .ToListAsync(); 
            return Results.Ok(genres);                                      
        });

        return group;
    }
}
