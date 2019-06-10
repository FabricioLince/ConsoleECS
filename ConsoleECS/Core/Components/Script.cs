

using System;

namespace ConsoleECS.Core.Components
{
    // Base abstract class to simplify the creation of components 
    public abstract class Script : ComponentBase
    {
        public abstract void Loop();

        public static void Message(string message, bool pause = true)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(message);
            if (pause) Console.ReadLine();
        }
    }
}
