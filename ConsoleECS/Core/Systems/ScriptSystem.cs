using System;
using ConsoleECS.Core.Components;

namespace ConsoleECS.Core.Systems
{
    class ScriptSystem : ComponentSystem<Script>
    {
        private Engine engine;

        public ScriptSystem(Engine engine)
        {
            this.engine = engine;
        }

        public override void Work(Script script)
        {
            script.Loop();
        }
    }
}
