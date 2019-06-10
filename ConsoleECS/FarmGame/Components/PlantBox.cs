using ConsoleECS.Core.Components;
using System;

namespace ConsoleECS.FarmGame.Components
{
    [Dependecies(typeof(Position))]
    class PlantBox : Script
    {
        [AssignDependence] Position position;
        public Crop crop;
        public bool HasCrop => crop != null;

        public bool Empty => !HasCrop;

        public bool Plant(Seed seed)
        {
            if (HasCrop) return false;
            var ent = EntityFactory.CreateCrop(position.Vector2Int, seed.kind);
            crop = ent.GetComponent<Crop>();
            return true;
        }
        public bool RemoveCrop()
        {
            if (!HasCrop) return false;
            Engine.DestroyEntity(crop.Entity);
            crop = null;
            return true;

        }

        public override void Loop()
        {
            
        }
    }
}
