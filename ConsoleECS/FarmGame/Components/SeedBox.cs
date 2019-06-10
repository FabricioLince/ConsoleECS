using ConsoleECS.Core.Components;

namespace ConsoleECS.FarmGame.Components
{
    [Dependecies(typeof(Position))]
    class SeedBox : Script
    {
        public Crop.Kind kind;
        [AssignDependence] Position position;

        Seed mySeed;
        Position mySeedPosition;

        public override void Loop()
        {
            if (mySeed == null || mySeedPosition.Vector2Int != position.Vector2Int)
            {
                var ent = EntityFactory.CreateSeed(position, kind);
                mySeed = ent.GetComponent<Seed>();
                mySeedPosition = ent.GetComponent<Position>();
            }
        }
    }
}
