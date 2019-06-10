using ConsoleECS.Core;
using ConsoleECS.Core.Components;
using System;

namespace ConsoleECS.FarmGame.Components
{
    [Dependecies(typeof(Renderer))]
    class Seed : HoldableItem
    {
        public Crop.Kind kind;
        [AssignDependence] Renderer renderer;

        public override void Loop()
        {
            base.Loop();

            if ((int)(Engine.Time * 5) % 2 == 0)
            {
                renderer.symbol = '.';
            }
            else
            {
                renderer.symbol = Crop.SymbolFor(kind)[0];
            }
        }
    }
}
