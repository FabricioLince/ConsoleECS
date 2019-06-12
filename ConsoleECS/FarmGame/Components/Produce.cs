using ConsoleECS.Core.Components;

namespace ConsoleECS.FarmGame.Components
{
    [Dependecies(typeof(Renderer))]
    public class Produce : HoldableItem
    {
        internal Crop.Kind kind;
        [AssignDependence] Renderer renderer;
    }
}
