using ConsoleECS.Core.Components;
using ConsoleECS.Core.Vector;
using System;
using System.Collections.Generic;

namespace ConsoleECS.Core.Systems
{
    public class ColliderSystem : ComponentSystem<Collider>
    {
        public Dictionary<int, Dictionary<int, bool>> layerCollisions = new Dictionary<int, Dictionary<int, bool>>();
        private Engine engine;

        public ColliderSystem(Engine engine)
        {
            this.engine = engine;
        }
        public override void Work() { }

        protected override void OnAddComponent(Collider c)
        {
            base.OnAddComponent(c);
            c.system = this;
        }

        public bool CheckCollision(Vector2Int position, int layerId = 0)
        {
            return components.Find(c => c.enabled && c.Position.Vector2Int.Equals(position)) != null;
        }
        public bool CanMoveTo(Collider collider, Vector2Int position)
        {
            // TODO check layer
            if (!collider.enabled) return true;
            var obstacle = components.Find(c => c.enabled && c.Position.Vector2Int.Equals(position));
            return (obstacle == null || obstacle == collider);
        }

        public bool CheckCollision(Collider c1, Collider c2)
        {
            return c1.Position.Equals(c2.Position);
        }

        public void SetCollisionForLayer(int layerA, int layerB, bool collide)
        {
            if (layerCollisions.ContainsKey(layerA) == false)
            {
                layerCollisions.Add(layerA, new Dictionary<int, bool>());
            }
            //if (layerCollisions[layerA].ContainsKey(layerB) == false) 
            layerCollisions[layerA][layerB] = collide;
        }

        public bool GetCollisionForLayer(int layerA, int layerB)
        {
            if (layerCollisions.ContainsKey(layerA) == false)
            {
                layerCollisions.Add(layerA, new Dictionary<int, bool>());
                
            }
            return layerCollisions[layerA].ContainsKey(layerB)
                    && layerCollisions[layerA][layerB];

        }
    }
}
