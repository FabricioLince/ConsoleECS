﻿using ConsoleECS.Core.Systems;
using ConsoleECS.Core.Vector;

namespace ConsoleECS.Core.Components
{
    [Dependecies(typeof(Position))]
    class Collider : ComponentBase
    {
        [AssignDependence]
        public Position position;

        public int layerId = 0;

        public ColliderSystem system;

        public bool Move(Vector2 direction, double distance)
        {
            while (distance > 0)
            {
                // incremental movement, 0.5 step
                if (distance > 0.5)
                {
                    distance -= 0.5;
                    var destino = position.Vector2 + direction * 0.5;
                    if (system.CanMoveTo(this, destino))
                    {
                        position.Vector2 = destino;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var destino = position.Vector2 + direction * distance;
                    if (!system.CanMoveTo(this, destino))
                    {
                        return false;
                    }
                    position.Vector2 = destino;
                    return true;
                }
            }
            return true;
        }
    }
}