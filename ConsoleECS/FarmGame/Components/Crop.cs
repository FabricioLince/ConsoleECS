using ConsoleECS.Core.Components;
using ConsoleECS.FarmGame.Components.GUI;
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

        public Produce Produce { get; private set; }

        public Kind kind { get; set; }
        double timeToDieFromThirst = 10;
        double timeToGrow = 10;

        double timer = 5;
        int stage = 0;
        public bool NeedsWater { get; private set; } = true;
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
        public override void OnCreate()
        {
            base.OnCreate();
            LogBox.Log(kind + " was planted on " + position);
        }

        public void WaterCrop()
        {
            if (!NeedsWater) return;

            NeedsWater = false;
            stage++;
            timer = timeToGrow;
        }

        public override void Loop()
        {
            timer -= Engine.DeltaTime;

            if(NeedsWater)
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
                    renderer.foregroundColor = ConsoleColor.Cyan;
                else
                    renderer.foregroundColor = ConsoleColor.DarkCyan;

                if (timer < 0)
                {
                    stage++;
                    NeedsWater = true;
                    timer = timeToDieFromThirst;
                }
            }

            renderer.symbol = Symbol;

            if (stage >= 6)
            {
                NeedsWater = false;

                if (Produce)
                {
                    if (Produce.HeldBy || Produce.Position.Vector2Int != position.Vector2Int)
                    {
                        Engine.DestroyEntity(this.Entity);
                        Produce = null;
                    }
                }
                else
                {
                    LogBox.Log(kind + " is ready to harvest");
                    var ent = EntityFactory.CreateProduce(position.Vector2Int, kind);
                    Produce = ent.GetComponent<Produce>();
                }
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
