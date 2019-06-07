using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleECS.Core
{
    public abstract class ComponentBase
    {
        protected Dictionary<System.Type, ComponentBase> dependencies;
        private bool missing = false;

        public Entity Entity { get; private set; }
        public Engine Engine { get; private set; }

        public virtual void OnCreate() { }

        private System.Type[] GetAllDependecies()
        {
            var attr = GetType().GetCustomAttribute<DependeciesAttribute>();
            if (attr != null)
                return attr.types;
            return null;
        }

        protected T GetDependencieOfType<T>() where T : ComponentBase
        {
            return dependencies[typeof(T)] as T;
        }

        public void AssignDependencies(Entity entity)
        {
            missing = false;
            Entity = entity;
            Engine = entity.Engine;

            var types = GetAllDependecies();
            if (types == null || types.Length == 0) return;

            dependencies = new Dictionary<Type, ComponentBase>();
            foreach (var type in types)
            {
                var otherComponent = entity.GetComponent(type);
                if (otherComponent == null)
                {
                    Console.WriteLine("Component " + this.GetType().Name + " requires " + type.Name);
                    Console.WriteLine("Make sure you're adding " + type.Name + " to the entity before adding " + GetType().Name);
                    missing = true;
                }
                else
                {
                    dependencies.Add(type, otherComponent);
                }
            }

            AssignDepenciesValuesOnFields();
        }
        public bool CheckDependencies()
        {
            return !missing;
        }
        protected void AssignDepenciesValuesOnFields()
        {
            //Console.WriteLine(this.GetType().Name);

            var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach(var field in fields)
            {
                //Console.WriteLine("\t" + field.FieldType + " " + field.Name);

                var att = field.GetCustomAttribute<AssignDependenceAttribute>();
                if (att != null)
                {
                    //Console.WriteLine("\t" + att);
                    if (dependencies.ContainsKey(field.FieldType))
                    {
                        var value = dependencies[field.FieldType];
                        field.SetValue(this, value);
                        //Console.WriteLine(value.Equals(field.GetValue(this)));
                    }
                    else Console.WriteLine(field.FieldType + " not found in dependencies");
                }
            }
        }

        public static bool operator!(ComponentBase c)
        {
            return (c == null);
        }
        public static implicit operator bool(ComponentBase c)
        {
            return (c != null);
        }

        /// <summary>
        /// Use this attribute to signal the engine to automatically assign the value of another component on this entity to this field
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public class AssignDependenceAttribute : Attribute
        {
        }
        
        /// <summary>
        /// Signals the engine that the creation of this Component depends on the previous existence of another component on the same entity
        /// If the dependency component is not found this Component will not be created
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class DependeciesAttribute : Attribute
        {
            public Type[] types;

            public DependeciesAttribute(params Type[] types)
            {
                this.types = types;
            }
        }
    }
}
