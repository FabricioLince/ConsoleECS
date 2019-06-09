using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.Core
{
    public interface IComponentSystem
    {
        void Work();
        System.Type ForType();
        void AddComponent(ComponentBase component);
        void Remove(ComponentBase component);
    }

    public abstract class ComponentSystem<Component> : IComponentSystem where Component : ComponentBase
    {
        public List<Component> components = new List<Component>();

        public void AddComponent(ComponentBase component)
        {
            var c = component as Component;
            if (c != null)
            {
                components.Add(c);
                OnAddComponent(c);
            }
            else
            {
                throw new Exception("Tried to AddComponent of wrong type to System " 
                    + component.GetType().ToString() + " to system for " + typeof(Component).ToString());
            }
        }
        protected virtual void OnAddComponent(Component c) { }
        public void Remove(ComponentBase c)
        {
            components.Remove(c as Component);
        }

        public Type ForType() { return typeof(Component); }


        public virtual void Work()
        {
            for (int i = 0; i < components.Count; ++i) 
            {
                Work(components[i]);
            }
        }
        public virtual void Work(Component c) { }
    }
}
