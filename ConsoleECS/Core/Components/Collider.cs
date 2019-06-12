using ConsoleECS.Core.Systems;
using ConsoleECS.Core.Vector;

namespace ConsoleECS.Core.Components
{
    [Dependecies(typeof(Position))]
    public class Collider : ComponentBase
    {
#pragma warning disable 649, 169
        [AssignDependence] public Position Position { get; private set; }
#pragma warning restore 649, 169

        public int layerId = 0;
        public bool enabled = true;

        public ColliderSystem system;

        public bool Move(Vector2 direction, double distance)
        {
            while (distance > 0)
            {
                // incremental movement, 0.5 step
                if (distance > 0.5)
                {
                    distance -= 0.5;
                    var destino = Position.Vector2 + direction * 0.5;
                    if (system.CanMoveTo(this, destino))
                    {
                        Position.Vector2 = destino;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var destino = Position.Vector2 + direction * distance;
                    if (!system.CanMoveTo(this, destino))
                    {
                        return false;
                    }
                    Position.Vector2 = destino;
                    return true;
                }
            }
            return true;
        }

        public bool CanMoveTo(Vector2Int position)
        {
            return system.CanMoveTo(this, position);
        }
    }
}
