using ConsoleECS.Core.Components;
using ConsoleECS.Core.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.Core.Systems
{
    class EnvironmentSystem : ComponentSystem<EnvironmentPiece>
    {
        Engine engine;
        private CameraSystem cameraSystem;

        public EnvironmentSystem(Engine engine)
        {
            this.engine = engine;
            cameraSystem = engine.GetSystem<CameraSystem>();
        }

        public override void Work()
        {
            // Draw then in the order they were added 
            // TODO : add layered ordering

            foreach (var piece in components)
            {
                var transformedPosition = cameraSystem.Transform(piece.position);
                engine.Screen.DrawRectangle(transformedPosition, piece.size, piece.backgroundColor, piece.foregroundColor, piece.symbol);
                //Console.SetCursorPosition(Console.BufferWidth - 10, 0);
                //Console.Write(piece.position);
            }
        }

        public ConsoleColor BackgroundColorOn(Vector2Int position)
        {
            foreach (var piece in components)
            {
                if (piece.Contains(position))
                {
                    return piece.backgroundColor;
                }
            }
            return Console.BackgroundColor;
        }
    }

    [Dependecies(typeof(Position))]
    public class EnvironmentPiece : ComponentBase
    {
        [AssignDependence]
        public Position position;

        public ConsoleColor backgroundColor = ConsoleColor.Black;
        public ConsoleColor foregroundColor = ConsoleColor.White;
        public char symbol = ' ';
        public Vector2Int size = Vector2Int.One;

        public bool Contains(Vector2Int point)
        {
            return point.x >= position.X && point.x < (position.X + size.x)
                && point.y >= position.Y && point.y < (position.Y + size.y);
        }
    }
}

