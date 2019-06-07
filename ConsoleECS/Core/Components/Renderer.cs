using System;

namespace ConsoleECS.Core.Components
{
    [Dependecies(typeof(Position))]
    class Renderer : ComponentBase
    {
        public char symbol = '?';
        public ConsoleColor foregroundColor = ConsoleColor.White;
#pragma warning disable 649, 169
        [AssignDependence]
        public Position Position;
#pragma warning restore 649, 169
    }
}
