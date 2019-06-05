using ConsoleECS.Core.Vector;
using System;

namespace ConsoleECS.Core.Components.GUI
{
    public abstract class GuiObject : ComponentBase
    {
        public Vector2Int position;
        public bool Show { get; protected set; }

        protected GuiObject()
        {
            Show = true;
        }

        public abstract void Draw();

        public enum Color
        {
            // Don't Change the buffer color
            Transparent = -1,
            Black = 0,
            DarkBlue = 1,
            DarkGreen = 2,
            DarkCyan = 3,
            DarkRed = 4,
            DarkMagenta = 5,
            DarkYellow = 6,
            Gray = 7,
            DarkGray = 8,
            Blue = 9,
            Green = 10,
            Cyan = 11,
            Red = 12,
            Magenta = 13,
            Yellow = 14,
            White = 15
        }
    }

}
