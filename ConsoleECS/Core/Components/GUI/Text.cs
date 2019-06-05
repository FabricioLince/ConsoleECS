using System;

namespace ConsoleECS.Core.Components.GUI
{
    class Text : GuiObject
    {
        public string text;

        public ConsoleColor foregroundColor = ConsoleColor.White;
        public ConsoleColor backgroundColor = ConsoleColor.Black;

        public override void Draw()
        {
            var x = position.x;
            var y = position.y;

            foreach(char c in text)
            {
                if (char.IsLetterOrDigit(c) ||
                    char.IsSymbol(c) ||
                    char.IsPunctuation(c) ||
                    c == ' ')
                {
                    Engine.Screen.SetBuffer(x, y, c, backgroundColor, foregroundColor);
                    x++;
                }
                else if (c == '\n') 
                {
                    y += 1;
                    x = position.x;
                }
            }
        }
    }
}
