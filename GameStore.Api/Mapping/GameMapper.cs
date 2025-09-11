using System;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api.Mapping;

public static class GameMapper
{
    // Extension method to map CreateGameDto to Game entity
    public static Game ToEntity(this CreateGameDto dto)
    {
        return new Game()
        {
            Name = dto.Name,
            GenreId = dto.GenreId,
            Price = dto.Price,
            ReleaseDate = dto.ReleaseDate
        };
    }

    public static Game ToEntity(this UpdateGameDto dto, int id)
    {
        return new Game()
        {
            Id = id,
            Name = dto.Name,
            GenreId = dto.GenreId,
            Price = dto.Price,
            ReleaseDate = dto.ReleaseDate
        };
    }

    // Extension method to map Game entity to GameDto
    public static GameDto ToDto(this Game game)
    {
        return new GameDto(
            game.Id,
            game.Name,
            game.Genre!.Name,
            game.Price,
            game.ReleaseDate
        );
    }

    public static GameDetailsDto ToGameDetailsDto(this Game game)
    {
        return new GameDetailsDto(
            game.Id,
            game.Name,
            game.GenreId,
            game.Price,
            game.ReleaseDate
        );
    }

}
