using ConsoleECS.Core.Components;
using System;
using System.Collections.Generic;

namespace ConsoleECS.FarmGame.Components
{
    [Dependecies(typeof(Position))]
    class PlantBox : Script
    {
        [AssignDependence] public Position Position { get; private set; }
        public Crop crop;
        public bool HasCrop => crop != null;

        public bool Empty => !HasCrop;

        public bool Plant(Seed seed)
        {
            if (HasCrop) return false;
            var ent = EntityFactory.CreateCrop(Position.Vector2Int, seed.kind);
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

        public override void OnCreate()
        {
            base.OnCreate();
            List.Add(this);
        }

        public static readonly List<PlantBox> List = new List<PlantBox>();
    }
}
