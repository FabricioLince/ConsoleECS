using System;

namespace ConsoleECS.Core.Input
{
    /// <summary>
    /// Codes representing keyboard keys.
    /// </summary>
    /// <remarks>
    /// Key code documentation:
    /// http://msdn.microsoft.com/en-us/library/dd375731%28v=VS.85%29.aspx
    /// </remarks>
    internal enum KeyCode : int
    {
        /// <summary>
        /// The left arrow key.
        /// </summary>
        Left = 0x25,

        /// <summary>
        /// The up arrow key.
        /// </summary>
        Up,

        /// <summary>
        /// The right arrow key.
        /// </summary>
        Right,

        /// <summary>
        /// The down arrow key.
        /// </summary>
        Down,

        CapsLock = 0x14,

        Alpha0 = 0x30,
        Alpha1,
        Alpha2,
        Alpha3,
        Alpha4,
        Alpha5,
        Alpha6,
        Alpha7,
        Alpha8,
        Alpha9,

        A = 0x41,
        B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    }

    /// <summary>
    /// Provides keyboard access.
    /// </summary>
    internal static class Keyboard
    {
        /// <summary>
        /// A positional bit flag indicating the part of a key state denoting
        /// key pressed.
        /// </summary>
        private const int KeyPressed = 0x8000;

        /// <summary>
        /// A positional bit flag indicating the part of a key state denoting
        /// key toggled, (think CapsLock ON)
        /// </summary>
        private const int KeyToggled = 0x0001;

        /// <summary>
        /// Returns a value indicating if a given key is pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>
        /// <c>true</c> if the key is pressed, otherwise <c>false</c>.
        /// </returns>
        public static bool IsKeyDown(KeyCode key)
        {
            return (GetKeyState((int)key) & KeyPressed) != 0;
        }

        public static bool IsKeyDown(char keyChar)
        {
            keyChar = char.ToUpper(keyChar);

            if (keyChar >= '0' && keyChar <= '9')
                return (GetKeyState((int)(keyChar)) & KeyPressed) != 0;

            if (keyChar >= 'A' && keyChar <= 'Z')
                return (GetKeyState((int)(keyChar)) & KeyPressed) != 0;

            throw new ArgumentException("Not a valid key character (" + keyChar + ")");
        }

        public static bool IsKeyToggled(KeyCode key)
        {
            return (GetKeyState((int)key) & KeyToggled) != 0;
        }

        /// <summary>
        /// Gets the key state of a key.
        /// </summary>
        /// <param name="key">Virtuak-key code for key.</param>
        /// <returns>The state of the key.</returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetKeyState(int key);
    }

}
