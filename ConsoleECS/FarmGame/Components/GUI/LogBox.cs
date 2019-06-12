using ConsoleECS.Core.Components;
using ConsoleECS.Core.Components.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.FarmGame.Components.GUI
{
    class LogBox : Script
    {
        public static Text textComponent;
        static List<string> messages = new List<string>();
        public static void Log(string message)
        {
            messages.Add(message);
        }

        public override void Loop()
        {
            if(messages.Count > 0)
            {
                if(messages.Count == 1)
                {
                    textComponent.text = messages[0];
                }
                else
                {
                    textComponent.text = messages[messages.Count - 2] + "\n" + messages[messages.Count - 1];
                }
            }
        }
    }
}
