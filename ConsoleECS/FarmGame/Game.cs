using ConsoleECS.Core;
using ConsoleECS.Core.Components;
using ConsoleECS.Core.Vector;
using ConsoleECS.FarmGame.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.FarmGame
{

    using Sound = Core.Sound.SoundModule;
    using Note = Core.Sound.Note;

    class Game
    {
        Engine engine;


        public void Run()
        {
            engine = new Engine();
            Console.WindowWidth = 40;
            Console.WindowHeight = 30;
            engine.Screen.UpdateBufferAllocation();

            Initialize();

            while (true)
            {
                engine.RunSystems();
            }
        }


        private void Initialize()
        {
            OuterWalls();
            CreatePlayer();

            PlantCrop(new Vector2Int(5, 5));
            PlantCrop(new Vector2Int(6, 5));
            PlantCrop(new Vector2Int(7, 5));

            PlantCrop(new Vector2Int(6, 8));
            PlantCrop(new Vector2Int(7, 8));
            PlantCrop(new Vector2Int(8, 8));
        }

        void PlantCrop(Vector2Int position)
        {
            var entity = engine.CreateEntity();
            entity.AddComponent<Position>().Vector2Int = position;
            entity.AddComponent<Renderer>();
            entity.AddComponent<Collider>();
            entity.AddComponent<Crop>();
        }

        void CreatePlayer()
        {
            var player = engine.CreateEntity();

            player.AddComponent<Position>().Vector2Int = new Vector2Int(2, 2);
            var renderer = player.AddComponent<Renderer>();
            renderer.symbol = 'A';
            renderer.foregroundColor = ConsoleColor.Blue;
            player.AddComponent<Collider>();
            player.AddComponent<Player>();
            player.GetComponent<Player>().speed = 10;
        }

        private void OuterWalls()
        {
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                CreateBlock(new Vector2Int(i, 0));
                CreateBlock(new Vector2Int(i, Console.WindowHeight - 2));
            }
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                CreateBlock(new Vector2Int(0, i));
                CreateBlock(new Vector2Int(Console.WindowWidth - 1, i));
            }
        }

        void CreateBlock(Vector2Int pos, char symbol = (char)9617)
        {
            var block = engine.CreateEntity();
            block.AddComponent<Position>().Vector2Int = pos;
            block.AddComponent<Collider>();
            if (symbol > 0)
            {
                block.AddComponent<Renderer>().symbol = symbol;
            }
        }

        public static void Main(string[] args)
        {
            new Game().Run();
        }
    }
}
