using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.Core
{
    public abstract class Scene
    {
        public Engine Engine { get; private set; }
        protected Dictionary<string, Entity> entities = new Dictionary<string, Entity>();
        public bool Run { get; protected set; }

        protected abstract void Load();
        protected virtual void Unload() { }

        public void Initiliaze(Engine engine)
        {
            this.Engine = engine;
            Load();
            Run = true;
        }
        public void Destroy()
        {
            if (Engine == null) return;
            Unload();
            foreach (var namedEntity in entities)
            {
                Engine.DestroyEntity(namedEntity.Value);
            }
            Engine = null;
            Run = false;
        }
        protected Entity CreateEntity(string name)
        {
            if (Engine == null) throw new Exception("Scene not yet initialized");
            
            var entity = Engine.CreateEntity(name);
            InsertEntity(entity);
            return entity;
        }
        public void InsertEntity(Entity entity)
        {
            var name = entity.Name;
            if (entities.ContainsKey(name))
            {
                int i = 2;
                while (entities.ContainsKey(name + "" + i)) i++;
                name += i;
            }
            entity.Name = name;
            entities.Add(name, entity);
        }
    }
}
