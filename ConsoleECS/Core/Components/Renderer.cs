using System;

namespace ConsoleECS.Core.Components
{
    [Dependecies(typeof(Position))]
    class Renderer : ComponentBase
    {
        public char symbol = '?';
        public ConsoleColor foregroundColor = ConsoleColor.White;

        [AssignDependence]
        public Position Position;
    }
}
