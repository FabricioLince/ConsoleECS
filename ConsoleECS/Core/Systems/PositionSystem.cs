using ConsoleECS.Core.Components;
using ConsoleECS.Core.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.Core.Systems
{
    class PositionSystem : ComponentSystem<Position>
    {
        private Engine engine;

        public PositionSystem(Engine engine)
        {
            this.engine = engine;
        }

        protected override void OnAddComponent(Position c)
        {
            base.OnAddComponent(c);
            c.system = this;
        }

        public override void Work()
        {}

        public List<Position> FindPosition(Vector2Int position)
        {
            return new List<Position>(components.Where(p => p.Vector2Int.Equals(position)));
        }
    }
}
