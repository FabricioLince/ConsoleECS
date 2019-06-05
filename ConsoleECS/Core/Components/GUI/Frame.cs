using ConsoleECS.Core.Vector;
using System;

namespace ConsoleECS.Core.Components.GUI
{

    public class Frame : GuiObject
    {
        public Vector2Int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                horSize = new Vector2Int(size.x - 1, 1);
                verSize = new Vector2Int(1, size.y - 1);
                interiorSize = new Vector2Int(size.x - 1, size.y - 1);
            }
        }
        Vector2Int size = Vector2Int.One;
        Vector2Int horSize = Vector2Int.Zero;
        Vector2Int verSize = Vector2Int.Zero;
        Vector2Int interiorSize = Vector2Int.Zero;

        public ConsoleColor backgroundColor = ConsoleColor.Black;
        public ConsoleColor foregroundColor = ConsoleColor.White;
        public char fill = ' ';
        public LineKind line;

        public enum LineKind { Single, Double, None }


        public override void Draw()
        {
            switch (line)
            {
                case LineKind.Single:
                    DrawSingleLine();
                    break;
                case LineKind.Double:
                    DrawDoubleLine();
                    break;
                case LineKind.None:
                    break;
            }
        }
        void DrawSingleLine()
        {
            Engine.Screen.SetBuffer(position.x, position.y, (char)FrameChar.UpperLeftCorner, backgroundColor, foregroundColor);
            Engine.Screen.SetBuffer(position.x + Size.x, position.y, (char)FrameChar.UpperRightCorner, backgroundColor, foregroundColor);
            Engine.Screen.SetBuffer(position.x, position.y + Size.y, (char)FrameChar.LowerLeftCorner, backgroundColor, foregroundColor);
            Engine.Screen.SetBuffer(position.x + Size.x, position.y + Size.y, (char)FrameChar.LowerRightCorner, backgroundColor, foregroundColor);

            Engine.Screen.DrawRectangle(position + new Vector2Int(1, 0), horSize, backgroundColor, foregroundColor, (char)FrameChar.HorizontalLine);
            Engine.Screen.DrawRectangle(position + new Vector2Int(1, Size.y), horSize, backgroundColor, foregroundColor, (char)FrameChar.HorizontalLine);

            Engine.Screen.DrawRectangle(position + new Vector2Int(0, 1), verSize, backgroundColor, foregroundColor, (char)FrameChar.VerticalLine);
            Engine.Screen.DrawRectangle(position + new Vector2Int(Size.x, 1), verSize, backgroundColor, foregroundColor, (char)FrameChar.VerticalLine);

            Engine.Screen.DrawRectangle(position + Vector2Int.One, interiorSize, backgroundColor, foregroundColor, fill);
        }

        void DrawDoubleLine()
        {
            Engine.Screen.SetBuffer(position.x, position.y, (char)DoubleLineFrameChar.UpperLeftCorner, backgroundColor, foregroundColor);
            Engine.Screen.SetBuffer(position.x + Size.x, position.y, (char)DoubleLineFrameChar.UpperRightCorner, backgroundColor, foregroundColor);
            Engine.Screen.SetBuffer(position.x, position.y + Size.y, (char)DoubleLineFrameChar.LowerLeftCorner, backgroundColor, foregroundColor);
            Engine.Screen.SetBuffer(position.x + Size.x, position.y + Size.y, (char)DoubleLineFrameChar.LowerRightCorner, backgroundColor, foregroundColor);

            Engine.Screen.DrawRectangle(position + new Vector2Int(1, 0), horSize, backgroundColor, foregroundColor, (char)DoubleLineFrameChar.HorizontalLine);
            Engine.Screen.DrawRectangle(position + new Vector2Int(1, Size.y), horSize, backgroundColor, foregroundColor, (char)DoubleLineFrameChar.HorizontalLine);

            Engine.Screen.DrawRectangle(position + new Vector2Int(0, 1), verSize, backgroundColor, foregroundColor, (char)DoubleLineFrameChar.VerticalLine);
            Engine.Screen.DrawRectangle(position + new Vector2Int(Size.x, 1), verSize, backgroundColor, foregroundColor, (char)DoubleLineFrameChar.VerticalLine);

            Engine.Screen.DrawRectangle(position + Vector2Int.One, interiorSize, backgroundColor, foregroundColor, fill);
        }

        public enum FrameChar
        {
            UpperLeftCorner = 9484,
            UpperRightCorner = 9488,
            LowerLeftCorner = 9492,
            LowerRightCorner = 9496,
            HorizontalLine = 9472,
            VerticalLine = 9474
        }
        public enum DoubleLineFrameChar
        {
            UpperLeftCorner = 9554,
            UpperRightCorner = 9557,
            LowerLeftCorner = 9560,
            LowerRightCorner = 9563,
            HorizontalLine = 9552,
            VerticalLine = 9553
        }
    }
}