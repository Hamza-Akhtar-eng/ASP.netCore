using GameStore.Client.Models;

namespace GameStore.Client;

public static class GameClient
{
    private static readonly List<Game> games = new(){
        new Game{
            Id=1,
            Name="street Fighter II",Genre="Fighting",Price=19.99M,ReleaseDate=new DateTime(1991,2,1)
        },new Game{
            Id=2,
            Name="Forza horizon 5",Genre="Driving",Price=21.99M,ReleaseDate=new DateTime(2021,2,1)
        },new Game{
            Id=3,
            Name="Smite",Genre="MMO/RPG",Price=25.99M,ReleaseDate=new DateTime(2008,2,1)
        },new Game{
            Id=4,
            Name="Fortnite",Genre="Multiplayer",Price=79.99M,ReleaseDate=new DateTime(2014,2,1)
        }
    };
    public static Game[] GetGames()
    {
        return games.ToArray();
    }
}
