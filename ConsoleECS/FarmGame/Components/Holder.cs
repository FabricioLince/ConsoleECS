using ConsoleECS.Core.Components;
using ConsoleECS.Core.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.FarmGame.Components
{
    class Holder : Script
    {
        public HoldableItem holding { get; private set; } = null;

        public void PickItem(HoldableItem item)
        {
            if (!holding)
            {
                item.PickUp(this.Entity);
                holding = item;
            }
        }
        public void DropItemOn(Vector2Int position)
        {
            if (holding)
            {
                holding.DropOn(position);
                holding = null;
            }
        }

        public override void Loop()
        {
        }
    }
}
