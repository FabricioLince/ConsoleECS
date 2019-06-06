using ConsoleECS.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.FarmGame.Components
{
    [Dependecies(typeof(Renderer))]
    public class Crop : Script
    {
        double age = 0;

        [AssignDependence]
        Renderer renderer;

        double timeToDieFromThirst = 5;
        double timeToGrow = 5;

        double timer = 5;
        int stage = 0;
        public bool needsWater = true;
        public bool dead = false;

        public void WaterCrop()
        {
            needsWater = false;
            stage++;
            timer = timeToGrow;
        }

        public override void Loop()
        {
            age += Engine.DeltaTime;
            timer -= Engine.DeltaTime;

            if(needsWater)
            {
                if (timer > timeToDieFromThirst * 0.3) 
                    renderer.foregroundColor = ConsoleColor.Gray;
                else
                    renderer.foregroundColor = ConsoleColor.DarkGray;
                
                if (timer < 0)
                {
                    dead = true;
                }
            }
            else
            {
                if (timer > timeToGrow * 0.3)
                    renderer.foregroundColor = ConsoleColor.Green;
                else
                    renderer.foregroundColor = ConsoleColor.DarkGreen;

                if (timer < 0)
                {
                    stage++;
                    needsWater = true;
                    timer = timeToDieFromThirst;
                }
            }
            switch(stage)
            {
                case 0:
                case 1:
                    renderer.symbol = '.';
                    break;
                case 2:
                case 3:
                    renderer.symbol = 't';
                    break;
                case 4:
                case 5:
                    renderer.symbol = 'T';
                    break;
                case 6:
                case 7:
                case 8:
                    renderer.foregroundColor = ConsoleColor.Red;
                    needsWater = false;
                    break;
                case 9:
                    renderer.foregroundColor = ConsoleColor.DarkRed ;
                    needsWater = false;
                    break;
                case 10:
                    dead = true;
                    break;
            }

            if(dead)
            {
                renderer.symbol = 'x';
                renderer.foregroundColor = ConsoleColor.DarkYellow;
            }
        }
    }
}
