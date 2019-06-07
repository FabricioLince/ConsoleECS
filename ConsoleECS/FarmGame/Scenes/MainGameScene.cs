using ConsoleECS.Core;
using ConsoleECS.Core.Components;
using ConsoleECS.Core.Vector;
using ConsoleECS.FarmGame.Components;
using System;

namespace ConsoleECS.FarmGame.Scenes
{
    class MainGameScene : Scene
    {
        protected override void Load()
        {
            EntityFactory.engine = Engine;
            OuterWalls();
            PrepareSoil();

            EntityFactory.CreatePlayer(new Vector2Int(2, 2));
            EntityFactory.CreateSeed(new Vector2Int(12, 8));

            PlantCrop(new Vector2Int(5, 5));
            PlantCrop(new Vector2Int(6, 5));
            PlantCrop(new Vector2Int(7, 5));

            PlantCrop(new Vector2Int(6, 8));
            PlantCrop(new Vector2Int(7, 8));
            PlantCrop(new Vector2Int(8, 8));

            //foreach (var ent in entities) Console.WriteLine(ent.Key);
            //Console.ReadKey();
        }

        void PrepareSoil()
        {
            EntityFactory.CreateSoil(new Vector2Int(5, 5), new Vector2Int(4, 2));
        }

        void PlantCrop(Vector2Int position)
        {
            
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
            var block = CreateEntity("Block");
            block.AddComponent<Position>().Vector2Int = pos;
            block.AddComponent<Collider>();
            if (symbol > 0)
            {
                block.AddComponent<Renderer>().symbol = symbol;
            }
        }

    }
}
