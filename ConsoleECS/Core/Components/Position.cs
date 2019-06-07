using ConsoleECS.Core.Systems;
using ConsoleECS.Core.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.Core.Components
{
    public class Position : ComponentBase
    {
        Vector2 position;
        internal PositionSystem system;

        public double X { get { return position.x; } set { position.x = value; } }
        public double Y { get { return position.y; } set { position.y = value; } }
        public Vector2 Vector2 { get { return position; } set { position = value; } }

        public Vector2Int Vector2Int { get { return position; } set { position = value; } }

        public void Translate(Vector2 delta)
        {
            position += delta;
        }

        public static implicit operator Vector2(Position p)
        {
            return p.position;
        }

        public override string ToString()
        {
            return "P:" + Vector2Int.x + "x" + Vector2Int.y;
        }

        public bool Equals(Position other)
        {
            return other != null &&
                Vector2Int.Equals(other.Vector2Int);
        }
    }
}
