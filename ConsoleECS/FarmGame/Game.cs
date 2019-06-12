using ConsoleECS.Core;
using ConsoleECS.FarmGame.Scenes;
using System;

namespace ConsoleECS.FarmGame
{
    // I wish there was a way to ignore these warnings automatically, wherever I put the [AssignDependence] attribute
#pragma warning disable 649, 169
    class Game
    {
        Engine engine;

        public void Run()
        {
            Console.WindowWidth = 40;
            Console.WindowHeight = 30;
            engine = new Engine();

            engine.StartWithScene<MainGameScene>();
        }

        
        public static void Main(string[] args)
        {
            try
            {
                new Game().Run();
            }
            catch (Exception e)
            {
                Console.BufferHeight = 150;
                Console.BufferWidth = 150;
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
                Console.ReadLine();
            }
            
        }
    }
}
