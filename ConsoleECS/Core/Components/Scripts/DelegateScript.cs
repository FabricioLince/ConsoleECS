using System;
using System.Collections.Generic;

namespace ConsoleECS.Core.Components.Scripts
{
    class DelegateScript : Script
    {
        public Action<DelegateScript> LoopFunction;
        public Dictionary<string, double> values = new Dictionary<string, double>();

        public override void Loop()
        {
            LoopFunction?.Invoke(this);
        }
    }
}
