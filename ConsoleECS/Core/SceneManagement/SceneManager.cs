using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.Core.SceneManagement
{
    public class SceneManager
    {
        Engine Engine { get; set; }
        public Scene CurrentScene { get; private set; }

        public SceneManager(Engine engine)
        {
            this.Engine = engine;
        }

        public void ChangeSceneTo<SceneType>() where SceneType : Scene, new()
        {
            if (CurrentScene != null)
            {
                CurrentScene.Destroy();
            }
            CurrentScene = new SceneType();
            CurrentScene.Initiliaze(Engine);

        }
    }
}
