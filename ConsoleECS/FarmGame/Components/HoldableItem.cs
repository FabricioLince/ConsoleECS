using ConsoleECS.Core;
using ConsoleECS.Core.Components;
using ConsoleECS.Core.Vector;
using System;

namespace ConsoleECS.FarmGame.Components
{
    [Dependecies(typeof(Position), typeof(Collider))]
    public abstract class HoldableItem : Script
    {
        [AssignDependence] public Position Position { get; set; }
        [AssignDependence] public Collider Collider { get; set; }

        Position holderPosition;

        public Entity HeldBy { get; set; }
        public bool DropOn(Vector2Int local)
        {
            if (Collider.system.CheckCollision(local)) return false;
            HeldBy = null;
            Position.Vector2Int = local;
            Collider.enabled = true;
            return true;
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
            Collider.enabled = false;
        }
        public void PickUp(ComponentBase c)
        {
            PickUp(c.Entity);
        }

        public override void Loop()
        {
            if (HeldBy)
            {
                Position.Vector2Int = holderPosition.Vector2Int + Vector2Int.Up;
            }
        }
    }
}
