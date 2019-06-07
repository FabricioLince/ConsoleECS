using ConsoleECS.Core.Components;
using ConsoleECS.Core.Input;
using ConsoleECS.Core.Systems;
using ConsoleECS.Core.Vector;
using System;

namespace ConsoleECS.Examples.Scripts
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
            camera.Delta = position.Vector2Int - (Engine.Screen.Size / 2);
        }
        public override void Loop()
        {
            if (!Keyboard.IsKeyToggled(KeyCode.CapsLock))
            {
                keyDown = false;
            }

            Vector2 movement = Vector2.Zero;

            if (Keyboard.IsKeyDown(upKey))
            {
                if (keyDown) return;
                keyDown = true;

                movement += Vector2.Up;
            }
            else if (Keyboard.IsKeyDown(downKey))
            {
                if (keyDown) return;
                keyDown = true;

                movement += Vector2.Down;
            }
            else if (Keyboard.IsKeyDown(leftKey))
            {
                if (keyDown) return;
                keyDown = true;

                movement += Vector2.Left;
            }
            else if (Keyboard.IsKeyDown(rightKey))
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
                collider.Move(movement, speed * Engine.DeltaTime);

                // center the camera on the player
                camera.Delta = position.Vector2Int - (Engine.Screen.Size / 2);
            }
        }
    }
}
