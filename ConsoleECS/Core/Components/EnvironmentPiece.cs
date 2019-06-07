using ConsoleECS.Core.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.Core.Components
{
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
