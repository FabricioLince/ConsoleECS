using ConsoleECS.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 649, 169
namespace ConsoleECS.FarmGame.Components
{
    [Dependecies(typeof(Position), typeof(Renderer))]
    public class Crop : Script
    {
        [AssignDependence] Position position;
        [AssignDependence] Renderer renderer;

        public Kind kind { get; set; }
        double timeToDieFromThirst = 10;
        double timeToGrow = 10;

        double timer = 5;
        int stage = 0;
        public bool needsWater = true;
        public bool Dead { get; private set; } = false;
        char Symbol
        {
            get
            {
                switch (stage)
                {
                    case 0:
                    case 1:
                        return '.';
                    case 2:
                    case 3:
                        return SymbolFor(kind)[0];
                    case 4:
                    case 5:
                        return SymbolFor(kind)[1];
                }
                return '%';
            }
        }


        public void WaterCrop()
        {
            if (!needsWater) return;

            needsWater = false;
            stage++;
            timer = timeToGrow;
        }

        public override void Loop()
        {
            timer -= Engine.DeltaTime;

            if(needsWater)
            {
                if (timer > timeToDieFromThirst * 0.3) 
                    renderer.foregroundColor = ConsoleColor.Gray;
                else
                    renderer.foregroundColor = ConsoleColor.DarkGray;
                
                if (timer < 0)
                {
                    Dead = true;
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
            renderer.symbol = Symbol;
            if (stage == 6)
            {
                EntityFactory.CreateProduce(position.Vector2Int, kind);
                Engine.DestroyEntity(this.Entity);
            }

            if(Dead)
            {
                renderer.symbol = 'x';
                renderer.foregroundColor = ConsoleColor.DarkYellow;
            }
        }



        public enum Kind
        {
            Tomato,
            Carrot,
            Lettuce,
        }
        public static char[] SymbolFor(Kind kind)
        {
            switch (kind)
            {
                case Kind.Tomato:
                    return "tT".ToArray();
                case Kind.Carrot:
                    return "cC".ToArray();
                case Kind.Lettuce:
                    return "lL".ToArray();
            }
            return "??".ToArray();
        }
        public static ConsoleColor ColorFor(Kind kind)
        {
            switch (kind)
            {
                case Kind.Tomato:
                    return ConsoleColor.Red;
                case Kind.Carrot:
                    return ConsoleColor.Yellow;
                case Kind.Lettuce:
                    return ConsoleColor.Green;
            }
            return ConsoleColor.White;
        }
    }
}
