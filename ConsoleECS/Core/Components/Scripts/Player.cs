using ConsoleECS.Core.Systems;
using ConsoleECS.Core.Vector;
using System;

namespace ConsoleECS.Core.Components.Scripts
{
    [Dependecies(typeof(Position), typeof(Collider))]
    class Player : Script
    {
        [AssignDependence]
        Position position;

        [AssignDependence]
        Collider collider;

        CameraSystem camera;

        public double speed = 20;

        bool keyDown = false;

        public KeyCode upKey = KeyCode.Up;
        public KeyCode downKey = KeyCode.Down;
        public KeyCode leftKey = KeyCode.Left;
        public KeyCode rightKey = KeyCode.Right;

        public override void OnCreate()
        {
            camera = Engine.GetSystem<CameraSystem>();
            //camera.Delta = position.Vector2Int - (Engine.Screen.Size / 2);
        }
        public override void Loop()
        {
            if (!NativeKeyboard.IsKeyToggled(KeyCode.CapsLock))
            {
                keyDown = false;
            }

            Vector2 movement = Vector2.Zero;

            if (NativeKeyboard.IsKeyDown(upKey))
            {
                if (keyDown) return;
                keyDown = true;

                movement += Vector2.Up;
            }
            else if (NativeKeyboard.IsKeyDown(downKey))
            {
                if (keyDown) return;
                keyDown = true;

                movement += Vector2.Down;
            }
            else if (NativeKeyboard.IsKeyDown(leftKey))
            {
                if (keyDown) return;
                keyDown = true;

                movement += Vector2.Left;
            }
            else if (NativeKeyboard.IsKeyDown(rightKey))
            {
                if (keyDown) return;
                keyDown = true;

                movement += (Vector2.Right);
            }
            else
            {
                keyDown = false;
            }

            if (Math.Abs(movement.x) > 0 ||
                Math.Abs(movement.y) > 0)
            {
                //if (!collider.Move(movement, speed * Engine.DeltaTime)) Console.Beep(740, 100);
                collider.Move(movement, speed * Engine.DeltaTime);

                // center the camera on the player
                //camera.Delta = position.Vector2Int - (Engine.Screen.Size / 2);
            }
        }
    }
}

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
internal static class NativeKeyboard
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

        throw new ArgumentException("Not a valid character (" + keyChar + ")");
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
