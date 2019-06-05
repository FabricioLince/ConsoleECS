using ConsoleECS.Core;
using ConsoleECS.Core.Components;
using ConsoleECS.Core.Components.GUI;
using ConsoleECS.Core.Components.Scripts;
using ConsoleECS.Core.Systems;
using ConsoleECS.Core.Vector;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleECS
{
    class Program
    {
        Engine engine = new Engine();

        public void Run()
        {
            Console.WindowHeight = 35;
            Console.WindowWidth = 35;
            engine.Screen.UpdateBufferAllocation();

            var hero = engine.CreateEntity();
            {
                hero.AddComponent<Position>();
                var renderer = hero.AddComponent<Renderer>();
                renderer.symbol = 'A';
                renderer.foregroundColor = ConsoleColor.Blue;
                hero.AddComponent<Collider>();
                hero.AddComponent<Player>();
            }

            var npc = engine.CreateEntity();
            {
                npc.AddComponent<Position>().Vector2 = new Vector2(10, 0);
                npc.AddComponent<Collider>();
                npc.AddComponent<Renderer>().symbol = 'n';
                npc.AddComponent<Npc>();
            }

            // Create various random rectangles to populate the area
            var random = new Random();
            char[] symbols = { ' ', '.', ',' };
            ConsoleColor[] fgColors = { ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.White };
            ConsoleColor[] bgColors = { ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.DarkYellow };
            for (int i = 0; i < 200; i++)
            {
                var pos = new Vector2Int(random.Next(0, 100), random.Next(0, 100));
                var size = new Vector2Int(random.Next(1, 16), random.Next(1, 16));
                var symbol = symbols[random.Next(symbols.Length)];
                var bgColor = bgColors[random.Next(bgColors.Length)];
                var fgColor = fgColors[random.Next(fgColors.Length)];
                CreateEnvPiece(pos, size, bgColor, fgColor, symbol);

                // 50% chance of the rectangle having collider 
                if (random.Next(2) == 0)
                    for (int x = 0; x < size.x; x++)
                    {
                        for (int y = 0; y < size.y; y++)
                        {
                            var c = engine.CreateEntity();
                            c.AddComponent<Position>().Vector2 = new Vector2(pos.x + x, pos.y + y);
                            c.AddComponent<Collider>();
                            c.AddComponent<Renderer>().symbol = (char)9617;
                        }
                    }
            }

            // Create an entity to represent a text message shown overlay
            {
                var guiEntity = engine.CreateEntity();
                guiEntity.Name = "guiEntity";
                var frame = guiEntity.AddComponent<Frame>();
                frame.position = new Vector2Int(1, engine.Screen.WindowHeight - 8);
                frame.Size = new Vector2Int(engine.Screen.WindowWidth - 3, 6); 
                frame.foregroundColor = ConsoleColor.DarkMagenta;
                frame.backgroundColor = ConsoleColor.Gray;
                frame.line = Frame.LineKind.Single;

                var text = guiEntity.AddComponent<Text>();
                text.position = new Vector2Int(2, engine.Screen.WindowHeight - 8 + 1);
                text.text = "Olá!\nMeu nome é Lince...";
                text.foregroundColor = ConsoleColor.DarkRed;
                text.backgroundColor = frame.backgroundColor;

                text = guiEntity.AddComponent<Text>();
                text.position = frame.position + frame.Size - new Vector2Int(1, 1);
                text.text = "X";
                text.foregroundColor = ConsoleColor.Black;
                text.backgroundColor = frame.backgroundColor;

                var scr = guiEntity.AddComponent<DelegateScript>();
                scr.LoopFunction = (script) =>
                {
                    if (NativeKeyboard.IsKeyDown(KeyCode.X))
                    {
                        while (NativeKeyboard.IsKeyDown(KeyCode.X)) Thread.Sleep(1);
                        engine.DestroyEntity(guiEntity);
                    }
                };
            }

            while (true)
            {
                engine.RunSystems();
            }
        }

        Entity CreateEnvPiece(Vector2Int position, Vector2Int size, ConsoleColor bgColor, ConsoleColor fgColor = ConsoleColor.White, char symbol = ' ')
        {
            var piece = engine.CreateEntity();
            piece.AddComponent<Position>().Vector2Int = position;

            var env = piece.AddComponent<EnvironmentPiece>();
            env.backgroundColor = bgColor;
            env.foregroundColor = fgColor;
            env.symbol = symbol;
            env.size = size;

            return piece;
        }

        static void Main(string[] args)
        {
            new Program().Run();
        }
    }
}
