using ConsoleECS.Core.Components;
using ConsoleECS.Core.Vector;
using System;
using System.Linq;

namespace ConsoleECS.Core
{
    public class Screen
    {
        public int WindowWidth { get; private set; }
        public int WindowHeight { get; private set; }
        public Vector2Int Size
        {
            get { return new Vector2Int(WindowWidth, WindowHeight); }
        }

        char[] screenBuffer = null;
        ConsoleColor[] foregroundColorBuffer = null;
        ConsoleColor[] backgroundColorBuffer = null;

        bool useColor = true;

        public Screen()
        {
            Console.Clear();
            Console.CursorVisible = false;
            UpdateBufferAllocation();
        }

        public void DrawRectangle(Vector2Int position, Vector2Int size, ConsoleColor bgColor, ConsoleColor fgColor = ConsoleColor.White, char symbol = ' ')
        {
            // Adjust rectangle size for rectangles positioned up and/or to the left of the screen

            while (size.x > 0 && !InsideWindowWidth(position.x))
            {
                position.x++;
                size.x--;
            }
            if (size.x <= 0) return;

            while (size.y > 0 && !InsideWindowHeight(position.y))
            {
                position.y++;
                size.y--;
            }
            if (size.y <= 0) return;

            // Draw the adjusted rectangle to the buffer

            for (int dy = 0; dy < size.y; ++dy)
            {
                if (InsideWindowHeight(position.y) == false) break;

                for (int dx = 0; dx < size.x; ++dx)
                {
                    var x = position.x + dx;
                    if (!InsideWindowWidth(x))
                        break;

                    SetBuffer(x, position.y, symbol);
                    SetBufferFgColor(x, position.y, fgColor);
                    SetBufferBgColor(x, position.y, bgColor);
                }

                position.y++;
            }
        }

        public void SetBufferFgColor(int x, int y, ConsoleColor color)
        {
            if (InsideBuffer(x, y))
                foregroundColorBuffer[x + y * WindowWidth] = color;
        }
        public void SetBufferBgColor(int x, int y, ConsoleColor color)
        {
            if (InsideBuffer(x, y))
                backgroundColorBuffer[x + y * WindowWidth] = color;
        }

        public void SetBuffer(int x, int y, char c)
        {
            if (InsideBuffer(x, y))
            {
                screenBuffer[x + y * WindowWidth] = c;
            }
        }
        public void SetBuffer(int x, int y, char c, ConsoleColor bgColor, ConsoleColor fgColor)
        {
            if (InsideBuffer(x, y))
            {
                screenBuffer[x + y * WindowWidth] = c;
                backgroundColorBuffer[x + y * WindowWidth] = bgColor;
                foregroundColorBuffer[x + y * WindowWidth] = fgColor;
            }
        }
        public void UpdateBufferAllocation()
        {
            if (screenBuffer != null 
                && (WindowHeight == Console.WindowHeight 
                && WindowWidth == Console.WindowWidth))
            {
                return;
            }

            if (screenBuffer == null) Console.SetWindowSize(80, 40);

            Console.CursorVisible = false;
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight+1;

            WindowWidth = Console.WindowWidth;
            WindowHeight = Console.WindowHeight;
            screenBuffer = new char[WindowHeight * WindowWidth -1];

            foregroundColorBuffer = new ConsoleColor[screenBuffer.Length];
            for (int i = 0; i < foregroundColorBuffer.Length; i++)
            {
                foregroundColorBuffer[i] = ConsoleColor.White;
            }
            backgroundColorBuffer = Enumerable.Repeat(ConsoleColor.Black, screenBuffer.Length).ToArray();

            //Console.SetCursorPosition(0, 0);
            //Console.WriteLine("New buffer is " + WindowWidth + "x" + WindowHeight + " = " + WindowHeight * WindowWidth + " chars long");
            //Console.ReadKey(true);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
        }

        public void Show()
        {
            UpdateBufferAllocation();
            //useColor = NativeKeyboard.IsKeyToggled(KeyCode.CapsLock);

            Console.SetCursorPosition(0, 0);
            if (useColor)
            {
                ConsoleColor currentBgColor = Console.BackgroundColor = backgroundColorBuffer[0];
                ConsoleColor currentFgColor = Console.ForegroundColor = foregroundColorBuffer[0];
                string smallBuffer = "" + screenBuffer[0];

                for (int i = 1; i < screenBuffer.Length;)
                {
                    while (currentBgColor == backgroundColorBuffer[i] && currentFgColor == foregroundColorBuffer[i])
                    {
                        smallBuffer += screenBuffer[i];

                        if (i >= screenBuffer.Length - 1)
                            break;
                        else
                            ++i;
                    }

                    Console.Write(smallBuffer);
                    smallBuffer = "";

                    if (i >= screenBuffer.Length - 1)
                        break;

                    if (currentBgColor != backgroundColorBuffer[i])
                    {
                        Console.BackgroundColor = currentBgColor = backgroundColorBuffer[i];
                    }
                    if (currentFgColor != foregroundColorBuffer[i])
                    {
                        Console.ForegroundColor = currentFgColor = foregroundColorBuffer[i];
                    }
                }
                Array.Clear(backgroundColorBuffer, 0, screenBuffer.Length);
                Array.Clear(foregroundColorBuffer, 0, screenBuffer.Length);
            }
            else
            {
                Console.Write(screenBuffer);
            }

            Array.Clear(screenBuffer, 0, screenBuffer.Length);
        }

        public bool InsideWindowWidth(int x)
        {
            return (x >= 0 && x < WindowWidth);
        }
        public bool InsideWindowHeight(int y)
        {
            return (y >= 0 && y < WindowHeight - 1);
        }
        public bool InsideBuffer(int x, int y)
        {
            return InsideWindowWidth(x) && InsideWindowHeight(y);
        }
        public bool InsideBuffer(Vector2Int position)
        {
            return InsideBuffer(position.x, position.y);
        }
        internal bool InsideBuffer(Position position)
        {
            return InsideBuffer(position.Vector2Int);
        }
    }
}
