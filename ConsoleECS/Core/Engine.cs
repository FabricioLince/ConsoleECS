using ConsoleECS.Core.Components;
using ConsoleECS.Core.Systems;
using ConsoleECS.Core.Vector;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleECS.Core
{
    public class Engine
    {
        List<IComponentSystem> systems = new List<IComponentSystem>();
        public Screen Screen { get; private set; }

        public T GetSystem<T>() where T : class, IComponentSystem
        {
            return systems.Find(s => s is T) as T;
        }
        public IComponentSystem GetSystem(Type componentType)
        {
            return systems.Find(s => componentType.Equals(s.ForType()) || componentType.IsSubclassOf(s.ForType()));
        }

        void OnAddComponent(Entity entity, ComponentBase component)
        {
            // search systems to insert the new component
            var componentType = component.GetType();
            var foundSystems = systems.FindAll(
                s =>
                    componentType.Equals(s.ForType()) ||
                    componentType.IsSubclassOf(s.ForType())
            );

            if (foundSystems.Count == 0)
            {
                //Console.WriteLine("No System for component " + component.GetType().ToString());
            }

            foreach (IComponentSystem system in foundSystems)
            {
                system.AddComponent(component);
                //Console.WriteLine("add " + component.GetType().Name + " in " + system.GetType().Name);
                //Console.ReadKey(true);
            }
        }
        public Entity CreateEntity()
        {
            var entity = new Entity(this);
            entity.OnAddComponent += OnAddComponent;
            return entity;
        }
        HashSet<ComponentBase> toDestroy = new HashSet<ComponentBase>();
        public void DestroyEntity(Entity entity)
        {
            foreach (var c in entity.components)
                toDestroy.Add(c);
        }
        public void DestroyComponent(ComponentBase component)
        {
            toDestroy.Add(component);
        }

        void DestroyPendingEntities()
        {
            if (toDestroy.Count == 0) return;

            foreach (var c in toDestroy)
            {
                var system = GetSystem(c.GetType());
                system?.Remove(c);
            }

            toDestroy.Clear();
        }

        public Engine()
        {
            Screen = new Screen();

            systems.Add(new CameraSystem(this));
            systems.Add(new ColliderSystem(this));
            systems.Add(new ScriptSystem(this));
            systems.Add(new EnvironmentSystem(this));
            systems.Add(new RendererSystem(this));
            systems.Add(new GuiLayerSystem(this));
        }

        public void RunSystems()
        {
            var currentTick = System.Environment.TickCount;
            if (currentTick - lastTick >= 1000)
            {
                FPS = frames;
                frames = 0;
                lastTick = currentTick;
            }
            frames++;

            DeltaTime = (currentTick - lastFrame) / 1000.0;
            lastFrame = currentTick;

            foreach (var system in systems)
            {
                system.Work();
            }
            Screen.Show();
            DestroyPendingEntities();

            Console.SetCursorPosition(0, Screen.WindowHeight - 1);
            Console.Write(FPS + "\t" + DeltaTime);
        }

        long lastTick;
        long lastFrame;
        long frames;
        public double DeltaTime { get; private set; }
        public long FPS { get; private set; }

    }


}