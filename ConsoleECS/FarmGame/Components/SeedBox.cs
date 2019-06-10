using ConsoleECS.Core.Components;
using System.Collections.Generic;

namespace ConsoleECS.FarmGame.Components
{
    [Dependecies(typeof(Position))]
    class SeedBox : Script
    {
        public Crop.Kind kind;
        [AssignDependence] public Position Position { get; private set; }

        Seed mySeed;
        Position mySeedPosition;

        public override void Loop()
        {
            if (mySeed == null || mySeedPosition.Vector2Int != Position.Vector2Int)
            {
                var ent = EntityFactory.CreateSeed(Position, kind);
                mySeed = ent.GetComponent<Seed>();
                mySeedPosition = ent.GetComponent<Position>();
            }
        }

        public override void OnCreate()
        {
            base.OnCreate();
            List.Add(this);
        }

        public static readonly List<SeedBox> List = new List<SeedBox>();
    }
}
