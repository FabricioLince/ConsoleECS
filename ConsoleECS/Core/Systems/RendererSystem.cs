using ConsoleECS.Core.Components;
namespace ConsoleECS.Core.Systems
{
    class RendererSystem : ComponentSystem<Renderer>
    {
        EnvironmentSystem envSystem;
        CameraSystem cameraSystem;
        Engine engine;

        public RendererSystem(Engine engine)
        {
            this.engine = engine;
            envSystem = engine.GetSystem<EnvironmentSystem>();
            cameraSystem = engine.GetSystem<CameraSystem>();
        }

        public override void Work(Renderer renderer)
        {
            var position = cameraSystem.Transform(renderer.Position);
            if (engine.Screen.InsideBuffer(position) == false) return;

            engine.Screen.SetBuffer(position.x, position.y, renderer.symbol);
            engine.Screen.SetBufferFgColor(position.x, position.y, renderer.foregroundColor);

            // unnecessary possible call to Console.BackgroundColor
            //engine.Screen.SetBufferBgColor(position.x, position.y, envSystem.BackgroundColorOn(position));
        }
    }
}
