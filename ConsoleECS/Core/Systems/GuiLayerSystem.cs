using ConsoleECS.Core.Components.GUI;

namespace ConsoleECS.Core.Systems
{
    class GuiLayerSystem : ComponentSystem<GuiObject>
    {
        private Engine engine;

        public GuiLayerSystem(Engine engine)
        {
            this.engine = engine;
        }

        public override void Work(GuiObject gui)
        {
            if (gui.Show)
                gui.Draw();
        }
    }
}
