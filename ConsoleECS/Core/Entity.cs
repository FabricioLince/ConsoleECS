using ConsoleECS.Core.Components;
using System;
using System.Collections.Generic;

namespace ConsoleECS.Core
{
    public class Entity
    {
        public string Name { get; set; }
        public List<ComponentBase> components = new List<ComponentBase>();
        public event Action<Entity, ComponentBase> OnAddComponent;
        public Engine Engine { get; private set; }

        /// <summary>
        /// Disposed means it was destroyed by the engine and shouldn't be used anymore 
        /// Converting to boolean and checking if == null will taking the Disposed status into consideration
        /// </summary>
        public bool Disposed { get; private set; }

        public Entity(Engine engine)
        {
            this.Engine = engine;
        }

        public T GetComponent<T>() where T : ComponentBase
        {
            return components.Find(c => c is T) as T;
        }

        internal ComponentBase GetComponent(Type type)
        {
            return components.Find(c => c.GetType().Equals(type));
        }

        public T AddComponent<T>() where T : ComponentBase, new()
        {
            if (Disposed) return null; // TODO: Show an error for trying to add components on a disposed Entity

            var component = new T();
            component.AssignDependencies(this);
            if (component.CheckDependencies())
            {
                components.Add(component);
                OnAddComponent?.Invoke(this, component);
                component.OnCreate();
                return component;
            }

            return null;
        }
        public void Dispose() => Disposed = true;

        public static implicit operator bool(Entity e)
        {
            return !(e is null) && !e.Disposed;
        }
        public static bool operator ==(Entity e, object o)
        {
            if (o == null)
            {
                return !e;
            }
            return e.Equals(o);
        }
        public static bool operator !=(Entity e, object o)
        {
            if (o == null)
            {
                return e;
            }
            return e.Equals(o);
        }
    }
}
