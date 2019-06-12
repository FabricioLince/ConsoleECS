using ConsoleECS.Core;
using ConsoleECS.Core.Components;
using ConsoleECS.Core.Components.GUI;
using ConsoleECS.Core.Components.Scripts;
using ConsoleECS.Core.Input;
using ConsoleECS.Core.Systems;
using ConsoleECS.Core.Vector;
using ConsoleECS.Examples.Scripts;
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
            Console.WindowHeight = 30;
            Console.WindowWidth = 40;
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

            hero = engine.CreateEntity();
            {
                hero.AddComponent<Position>().Vector2Int = new Vector2Int(2, 0);
                var renderer = hero.AddComponent<Renderer>();
                renderer.symbol = 'B';
                renderer.foregroundColor = ConsoleColor.Cyan;
                hero.AddComponent<Collider>();
                var player = hero.AddComponent<Player>();
                player.upKey = KeyCode.I;
                player.downKey = KeyCode.K;
                player.leftKey = KeyCode.J;
                player.rightKey = KeyCode.L;
            }

            var npc = engine.CreateEntity();
            {
                npc.AddComponent<Position>().Vector2 = new Vector2(10, 0);
                npc.AddComponent<Collider>();
                npc.AddComponent<Renderer>().symbol = 'n';
                npc.AddComponent<Npc>();
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

        public static Entity CreateMessageBox(Engine engine, string message)
        {
            var guiEntity = engine.CreateEntity();
            guiEntity.Name = "guiEntity";
            var frame = guiEntity.AddComponent<Frame>();
            frame.position = new Vector2Int(1, engine.Screen.WindowHeight - 6);
            frame.Size = new Vector2Int(engine.Screen.WindowWidth - 3, 4);
            frame.foregroundColor = ConsoleColor.DarkMagenta;
            frame.backgroundColor = ConsoleColor.Gray;
            frame.line = Frame.LineKind.Single;

            var text = guiEntity.AddComponent<Text>();
            text.position = new Vector2Int(2, engine.Screen.WindowHeight - 6 + 1);
            text.text = message;
            text.foregroundColor = ConsoleColor.DarkRed;
            text.backgroundColor = frame.backgroundColor;
            /*
            text = guiEntity.AddComponent<Text>();
            text.position = frame.position + frame.Size - new Vector2Int(1, 1);
            text.text = "X";
            text.foregroundColor = ConsoleColor.Black;
            text.backgroundColor = frame.backgroundColor;
            
            var scr = guiEntity.AddComponent<DelegateScript>();
            scr.LoopFunction = (script) =>
            {
                if (Keyboard.IsKeyDown(KeyCode.X))
                {
                    while (Keyboard.IsKeyDown(KeyCode.X)) Thread.Sleep(1);
                    engine.DestroyEntity(guiEntity);
                }
                if (((int)(script.Engine.Time * 2)) % 2 == 0)
                {
                    text.text = "";
                }
                else
                {
                    text.text = "X";
                }
            };
            */
            return guiEntity;
        }

        void Main(string[] args)
        {
            new Program().Run();
        }
    }
}
