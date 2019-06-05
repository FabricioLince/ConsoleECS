using System;

namespace ConsoleECS.Core.Vector
{
    public struct Vector2Int : IEquatable<Vector2Int>
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static readonly Vector2Int Up = new Vector2Int(0, -1);
        public static readonly Vector2Int Down = new Vector2Int(0, 1);
        public static readonly Vector2Int Left = new Vector2Int(-1, 0);
        public static readonly Vector2Int Right = new Vector2Int(1, 0);
        public static readonly Vector2Int One = new Vector2Int(1, 1);
        public static readonly Vector2Int Zero = new Vector2Int(0, 0);

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2Int)) return false;
            return this == (Vector2Int)obj;
        }

        public bool Equals(Vector2Int other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }

        public static Vector2Int operator +(Vector2Int v1, Vector2Int v2)
        {
            return new Vector2Int(v1.x + v2.x, v1.y + v2.y);
        }
        public static Vector2Int operator -(Vector2Int v1, Vector2Int v2)
        {
            return new Vector2Int(v1.x - v2.x, v1.y - v2.y);
        }
        public static Vector2Int operator *(Vector2Int v, int scale)
        {
            return new Vector2Int(v.x * scale, v.y * scale);
        }
        public static Vector2Int operator /(Vector2Int v, int scale)
        {
            return new Vector2Int(v.x / scale, v.y / scale);
        }
        public static bool operator ==(Vector2Int v1, Vector2Int v2)
        {
            return
                v1.x == v2.x &&
                v1.y == v2.y;
        }
        public static bool operator !=(Vector2Int v1, Vector2Int v2)
        {
            return
                v1.x != v2.x ||
                v1.y != v2.y;
        }
    }
}
