using ConsoleECS.Core;
using ConsoleECS.Core.Components;
using ConsoleECS.Core.SceneManagement;
using ConsoleECS.Core.Vector;
using ConsoleECS.FarmGame.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.FarmGame
{
    class EntityFactory
    {
        public static Engine engine;
        static SceneManager SceneManager => engine.SceneManager;

        static Entity CreateEntity(string name)
        {
            var entity = engine.CreateEntity(name);
            SceneManager.CurrentScene.InsertEntity(entity);
            return entity;
        }

        public static Entity CreateSeedBox(Vector2Int position)
        {
            var entity = CreateEntity("SeedBox");
            entity.AddComponent<Position>().Vector2Int = position;
            entity.AddComponent<Renderer>();
            entity.AddComponent<Collider>();
            entity.AddComponent<SeedBox>();

            return entity;
        }
        public static Entity CreateSeed(Vector2Int position)
        {
            var entity = CreateEntity("Seed");
            entity.AddComponent<Position>().Vector2Int = position;
            entity.AddComponent<Renderer>();
            entity.AddComponent<Collider>();
            entity.AddComponent<Seed>();

            return entity;
        }

        public static Entity CreateSoil(Vector2Int position, Vector2Int size)
        {
            var entity = CreateEntity("Soil");
            entity.AddComponent<Position>().Vector2Int = position;
            var env = entity.AddComponent<EnvironmentPiece>();
            env.size = size;
            env.backgroundColor = ConsoleColor.DarkGreen;
            env.symbol = '\'';

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var f = CreateEntity("SoilBox");
                    f.AddComponent<Position>().Vector2Int = position + new Vector2Int(x, y);
                    f.AddComponent<PlantBox>();
                }
            }

            return entity;
        }
        public static Entity CreateCrop(Vector2Int position)
        {
            var entity = CreateEntity("Crop");
            entity.AddComponent<Position>().Vector2Int = position;
            entity.AddComponent<Renderer>();
            entity.AddComponent<Collider>();
            entity.AddComponent<Crop>();
            return entity;
        }



        public static Entity CreatePlayer(Vector2Int position)
        {
            var player = CreateEntity("Player");

            player.AddComponent<Position>().Vector2Int = position;
            var renderer = player.AddComponent<Renderer>();
            renderer.symbol = 'A';
            renderer.foregroundColor = ConsoleColor.Blue;
            player.AddComponent<Collider>();
            player.AddComponent<Player>();
            player.GetComponent<Player>().speed = 10;

            return player;
        }
    }
}
