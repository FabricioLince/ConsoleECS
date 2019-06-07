using ConsoleECS.Core;
using ConsoleECS.Core.Components;
using ConsoleECS.Core.Vector;
using System;

namespace ConsoleECS.FarmGame.Components
{
    public interface IHoldable
    {
        Entity HeldBy { get; set; }
        void DropOn(Vector2Int position);
        void PickUp(Entity entity);
    }

    [Dependecies(typeof(Position), typeof(Renderer), typeof(Collider))]
    class Seed : Script, IHoldable
    {
        [AssignDependence]
        Renderer renderer;
        [AssignDependence]
        Position position;
        [AssignDependence]
        Collider collider;

        Position holderPosition;
        public Entity HeldBy { get; set; }
        public void DropOn(Vector2Int local)
        {
            HeldBy = null;
            position.Vector2Int = local;
            collider.enabled = true;
        }
        public void PickUp(Entity p)
        {
            HeldBy = p;
            holderPosition = HeldBy.GetComponent<Position>();
            collider.enabled = false;
        }

        public override void Loop()
        {
            if (HeldBy)
            {
                position.Vector2Int =  holderPosition.Vector2Int + Vector2Int.Up;
            }

            if ((int)(Engine.Time * 5) % 2 == 0)
            {
                renderer.symbol = '.';
            }
            else
            {
                renderer.symbol = 't';
            }
        }
    }
}
