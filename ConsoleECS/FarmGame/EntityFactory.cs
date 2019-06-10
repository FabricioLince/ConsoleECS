using ConsoleECS.Core;
using ConsoleECS.Core.Components;
using ConsoleECS.Core.SceneManagement;
using ConsoleECS.Core.Vector;
using ConsoleECS.FarmGame.Components;
using System;

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

        public static Entity CreateSeedBox(Vector2Int position, Crop.Kind kind)
        {
            var entity = CreateEntity("SeedBox", position);
            entity.AddComponent<SeedBox>().kind = kind;

            return entity;
        }
        public static Entity CreateSeed(Vector2Int position, Crop.Kind kind)
        {
            var entity = CreateEntity("Seed", position);
            entity.AddComponent<Seed>().kind = kind;

            return entity;
        }
        public static Entity CreateCrop(Vector2Int position, Crop.Kind kind)
        {
            var entity = CreateEntity("Crop", position);
            entity.AddComponent<Crop>().kind = kind;
            return entity;
        }
        public static Entity CreateProduce(Vector2Int position, Crop.Kind kind)
        {
            var ent = CreateEntity("Produce", position);
            ent.AddComponent<Produce>().kind = kind;
            ent.GetComponent<Renderer>().symbol = Crop.SymbolFor(kind)[1];
            ent.GetComponent<Renderer>().foregroundColor = Crop.ColorFor(kind);
            return ent;
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

        public static Entity CreateEntity(string name, Vector2Int position, bool collider = true)
        {
            var entity = CreateEntity(name);
            entity.AddComponent<Position>().Vector2Int = position;
            entity.AddComponent<Renderer>();
            if (collider)
            {
                entity.AddComponent<Collider>();
            }

            return entity;
        }
    }
}
