

namespace ConsoleECS.Core.Components
{
    // Base abstract class to simplify the creation of components 
    public abstract class Script : ComponentBase
    {
        public abstract void Loop();
    }
}
