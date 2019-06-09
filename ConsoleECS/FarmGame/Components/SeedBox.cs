using ConsoleECS.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.FarmGame.Components
{
    [Dependecies(typeof(Position))]
    class SeedBox : Script
    {
        [AssignDependence] Position position;

        Seed mySeed;
        Position mySeedPosition;

        public override void Loop()
        {
            if (mySeed == null || mySeedPosition.Vector2Int != position.Vector2Int)
            {
                var ent = EntityFactory.CreateSeed(position);
                mySeed = ent.GetComponent<Seed>();
                mySeedPosition = ent.GetComponent<Position>();
            }
        }
    }
}
