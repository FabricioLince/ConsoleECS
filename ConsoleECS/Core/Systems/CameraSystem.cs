using ConsoleECS.Core.Components;
using ConsoleECS.Core.Vector;

namespace ConsoleECS.Core.Systems
{
    class CameraSystem : ComponentSystem<CameraSystem.NoComponent>
    {
        internal class NoComponent : ComponentBase { }

        private Engine engine;

        public CameraSystem(Engine engine)
        {
            this.engine = engine;
            Delta = Vector2Int.Zero;
        }

        public override void Work()
        {
            // Move the camera independently using WASD keys
            // This is for testing purposes
            // Ideally you should move the camera in a component of your choosing

            if (NativeKeyboard.IsKeyDown(KeyCode.W))
            {
                Delta += Vector2Int.Up;
            }
            else if (NativeKeyboard.IsKeyDown(KeyCode.S))
            {
                Delta += Vector2Int.Down;
            }
            else if (NativeKeyboard.IsKeyDown(KeyCode.A))
            {
                Delta += Vector2Int.Left;
            }
            else if (NativeKeyboard.IsKeyDown(KeyCode.D))
            {
                Delta += Vector2Int.Right;
            }
        }

        /// <summary>
        /// Transforms a Position in World Space to Screen Space
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2Int Transform(Position position)
        {
            return position.Vector2Int - Delta;
        }

        /// <summary>
        /// The Distance, in integer units, the camera is from the world origin (0, 0)
        /// </summary>
        public Vector2Int Delta
        {
            get; set;
        }
    }
}
