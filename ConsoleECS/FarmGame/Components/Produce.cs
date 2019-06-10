using ConsoleECS.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.FarmGame.Components
{
    [Dependecies(typeof(Renderer))]
    class Produce : HoldableItem
    {
        internal Crop.Kind kind;
        [AssignDependence] Renderer renderer;
    }
}
