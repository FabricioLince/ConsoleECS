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
        public bool DropItemOn(Vector2Int position)
        {
            if (holding)
            {
                if (holding.DropOn(position))
                {
                    holding = null;
                }
            }
            return !holding;
        }

        public override void Loop()
        {
        }
    }
}
