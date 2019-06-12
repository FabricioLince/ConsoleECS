using ConsoleECS.Core.Components;
using System;
using System.Collections.Generic;

namespace ConsoleECS.FarmGame.Components
{
    [Dependecies(typeof(Position))]
    class PlantBox : Script
    {
        [AssignDependence] public Position Position { get; private set; }
        public Crop Crop { get; private set; }
        public Produce Produce => Crop?.Produce;
        public bool HasCrop => Crop;
        public bool HasProduce => Produce;

        public bool Empty => !HasCrop && !HasProduce;

        public bool Plant(Seed seed)
        {
            if (HasCrop) return false;
            var ent = EntityFactory.CreateCrop(Position.Vector2Int, seed.kind);
            Crop = ent.GetComponent<Crop>();
            return true;
        }
        public bool RemoveCrop()
        {
            if (!HasCrop) return false;
            Engine.DestroyEntity(Crop.Entity);
            Crop = null;
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
