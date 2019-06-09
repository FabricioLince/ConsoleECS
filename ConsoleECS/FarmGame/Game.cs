using ConsoleECS.Core;
using ConsoleECS.FarmGame.Scenes;
using System;

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
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
                Console.ReadKey();
            }
        }
    }
}
