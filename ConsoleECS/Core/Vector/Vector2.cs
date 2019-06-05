using System;

namespace ConsoleECS.Core.Vector
{

    public struct Vector2 : IEquatable<Vector2>
    {
        public double x;
        public double y;

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static readonly Vector2 Up = new Vector2(0, -1);
        public static readonly Vector2 Down = new Vector2(0, 1);
        public static readonly Vector2 Left = new Vector2(-1, 0);
        public static readonly Vector2 Right = new Vector2(1, 0);
        public static readonly Vector2 One = new Vector2(1, 1);
        public static readonly Vector2 Zero = new Vector2(0, 0);

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2)) return false;
            return this == (Vector2)obj;
        }

        public bool Equals(Vector2 other)
        {
            return this == other;
        }

        public double Magnitude => Math.Sqrt(x * x + y * y);

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }
        public static Vector2 operator *(Vector2 v, double scale)
        {
            return new Vector2(v.x * scale, v.y * scale);
        }
        public static Vector2 operator /(Vector2 v, double scale)
        {
            return new Vector2(v.x / scale, v.y / scale);
        }
        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return
                v1.x == v2.x &&
                v1.y == v2.y;
        }
        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return
                v1.x != v2.x ||
                v1.y != v2.y;
        }

        public static implicit operator Vector2Int(Vector2 v)
        {
            return new Vector2Int((int)v.x, (int)v.y);
        }

        public static implicit operator Vector2(Vector2Int v)
        {
            return new Vector2(v.x, v.y);
        }

        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }
    }
}
