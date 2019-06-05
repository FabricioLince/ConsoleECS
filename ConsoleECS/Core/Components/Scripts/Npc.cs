using ConsoleECS.Core.Vector;
using System;

namespace ConsoleECS.Core.Components.Scripts
{
    [Dependecies(typeof(Position), typeof(Collider))]
    class Npc : Script
    {
        // I wish there was a way to ignore these warnings automatically, wherever I put the [AssignDependence] attribute
#pragma warning disable 649, 169
        [AssignDependence]
        Position position;
        [AssignDependence]
        Collider collider;
#pragma warning restore 649, 169

        double timer = 0;
        Random random = new Random();
        Vector2 direction = Vector2.Up;

        public override void Loop()
        {
            if (timer < 0)
            {
                var directions = new Vector2[] { Vector2.Up, Vector2.Down, Vector2.Right, Vector2.Left };
                direction = directions[random.Next(directions.Length)];
                timer = 1;
            }
            timer -= Engine.DeltaTime;

            collider.Move(direction, 3 * Engine.DeltaTime);
        }
    }
}
