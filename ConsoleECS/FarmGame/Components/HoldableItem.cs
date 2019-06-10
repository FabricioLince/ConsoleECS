using ConsoleECS.Core;
using ConsoleECS.Core.Components;
using ConsoleECS.Core.Vector;
using System;

namespace ConsoleECS.FarmGame.Components
{
    /*
    public interface IHoldable
    {
        Entity HeldBy { get; set; }
        void DropOn(Vector2Int position);
        void PickUp(Entity entity);
    }*/

    [Dependecies(typeof(Position), typeof(Collider))]
    public abstract class HoldableItem : Script
    {
        [AssignDependence] readonly Position position;
        [AssignDependence] Collider collider;
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
            if (holderPosition == null) 
            {
                HeldBy = null;
                return;
            }
            collider.enabled = false;
        }
        public void PickUp(ComponentBase c)
        {
            PickUp(c.Entity);
        }

        public override void Loop()
        {
            if (HeldBy)
            {
                position.Vector2Int = holderPosition.Vector2Int + Vector2Int.Up;
            }
        }
    }
}
