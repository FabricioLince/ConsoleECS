using ConsoleECS.Core;
using ConsoleECS.Core.Components;
using ConsoleECS.Core.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable 649, 169

namespace ConsoleECS.FarmGame.Components
{
    [Dependecies(typeof(Position), typeof(Collider), typeof(Holder))]
    class Npc : Script
    {
        [AssignDependence] public Position Position { get; private set; }
        [AssignDependence] public Collider Collider { get; private set; }
        [AssignDependence] public Holder Holder { get; private set; }
        [AssignDependence] public Renderer Renderer { get; private set; }

        public Vector2Int direction = Vector2Int.Up;
        public double speed = 10;
        //Dictionary<Vector2Int, TileKind> MentalMap = new Dictionary<Vector2Int, TileKind>();

        Vector2Int Front => Position.Vector2Int + direction;

        Random random = new Random();

        public override void Loop()
        {
            if (Holder.holding)
            {
                Renderer.symbol = 'h';
                if (Holder.holding is Seed)
                {
                    var pBox = ComponentInFront<PlantBox>();
                    if (pBox && pBox.Empty)
                    {
                        pBox.Plant(Holder.holding as Seed);
                        Engine.DestroyEntity(Holder.holding.Entity);
                    }
                    else
                    {
                        var destPlantBox = PlantBox.List.Find(pb => pb.Empty);
                        if (destPlantBox)
                        {
                            MoveTo(destPlantBox.Position);
                        }
                        else
                        {
                            if (!Holder.DropItemOn(Front))
                            {
                                Collider.Move(RandomDirection, speed * Engine.DeltaTime);
                            }
                        }
                    }
                }
                else if(Holder.holding is Produce)
                {
                    MoveTo(Vector2Int.One);
                }
            }
            else
            {
                var needsWater = PlantBox.List.Find(pb => pb.Crop && pb.Crop.NeedsWater);
                if (needsWater)
                {
                    Renderer.symbol = 'r';
                    var pbInFront = ComponentInFront<PlantBox>();
                    if (pbInFront && pbInFront.Crop && pbInFront.Crop.NeedsWater) 
                    {
                        pbInFront.Crop.WaterCrop();
                    }
                    else
                    {
                        MoveTo(needsWater.Position);
                    }
                }
                else if (PlantBox.List.Find(pb => pb.Empty))
                {
                    Renderer.symbol = 'p';
                    var destSeedBox = SeedBox.List[random.Next(SeedBox.List.Count)];
                    var seedInFront = ComponentInFront<Seed>();
                    if (seedInFront)
                    {
                        Holder.PickItem(seedInFront);
                    }
                    else
                    {
                        MoveTo(destSeedBox.Position);
                    }
                }
                else
                {
                    var produceReady = PlantBox.List.Find(pb => pb.HasProduce);
                    if(produceReady)
                    {
                        var produceInFront = ComponentInFront<Produce>();
                        if (produceInFront)
                        {
                            Holder.PickItem(produceInFront);
                        }
                        else
                        {
                            MoveTo(produceReady.Position);
                        }
                    }
                }
            }
        }

        void MoveTo(Vector2Int dest)
        {
            var dx = Position.Vector2Int.x - dest.x;
            var dy = Position.Vector2Int.y - dest.y;

            if (dx == 0 || random.NextDouble() > 0.5)
            {
                if (dy == 0)
                {
                }
                else if (dy < 0)
                {
                    direction = Vector2Int.Down;
                }
                else
                {
                    direction = Vector2Int.Up;
                }
            }
            else if (dx < 0)
            {
                direction = Vector2Int.Right;
            }
            else
            {
                direction = Vector2Int.Left;
            }

            var colInFront = ComponentInFront<Collider>();
            if (colInFront && colInFront.enabled)
            {
                Collider.Move(RandomDirection, speed * Engine.DeltaTime);
            }
            Collider.Move(direction, speed * Engine.DeltaTime);
        }
        Component ComponentInFront<Component>() where Component : ComponentBase
        {
            var list = Position.system.FindPosition(Position.Vector2Int + direction);
            foreach (var pos in list)
            {
                var ent = pos.Entity.GetComponent<Component>();
                if (ent) return ent;
            }
            return null;
        }

        Vector2Int RandomDirection => 
            new Vector2Int[]
            {
                Vector2Int.Down,
                Vector2Int.Up,
                Vector2Int.Left,
                Vector2Int.Right
            }
            [random.Next(4)];

        enum TileKind
        {
            EmptySpace,
            PlantBox,
            SeedBox
        }
    }
}