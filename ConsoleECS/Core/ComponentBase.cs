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
        /// <summary>
        /// Disposed means it was destroyed by the engine and shouldn't be used anymore 
        /// Converting to boolean and checking if == null will taking the Disposed status into consideration
        /// </summary>
        public bool Disposed { get; private set; }
        public virtual void OnCreate() { }

        private List<Type> GetAllDependecies()
        {
            var list = new List<Type>();
            var attr = GetType().GetCustomAttributes<DependeciesAttribute>();
            if (attr != null)
            {
                foreach (var item in attr)
                {
                    list.AddRange(item.types);
                }
            }

            return list;
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
            if (types == null || types.Count == 0) return;

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
            var type = GetType();
            while (type != null)
            {
                var fields = type.GetFields( BindingFlags.SetProperty| BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var field in fields)
                {
                    //Console.WriteLine("\t" + field.FieldType + " " + field.Name);

                    var att = field.GetCustomAttribute<AssignDependenceAttribute>();
                    if (att != null)
                    {
                        //Console.WriteLine("\t" + att);
                        //if (dependencies.ContainsKey(field.FieldType))
                        {
                            var value = dependencies[field.FieldType];
                            field.SetValue(this, value);
                            //Console.WriteLine(value.Equals(field.GetValue(this)));
                        }
                        //else throw new Exception(field.FieldType + " not found in dependencies of " + type.FullName);
                    }
                }
                var properties = type.GetProperties(BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var prop in properties)
                {
                    //Console.WriteLine("\t" + field.FieldType + " " + field.Name);

                    var att = prop.GetCustomAttribute<AssignDependenceAttribute>();
                    if (att != null)
                    {
                        //Console.WriteLine("\t" + att);
                        //if (dependencies.ContainsKey(field.FieldType))
                        {
                            var value = dependencies[prop.PropertyType];
                            prop.SetValue(this, value);
                            //Console.WriteLine(value.Equals(field.GetValue(this)));
                        }
                        //else throw new Exception(field.FieldType + " not found in dependencies of " + type.FullName);
                    }
                }
                type = type.BaseType;
            }
        }

        public void Dispose() => Disposed = true;

        public static implicit operator bool(ComponentBase c)
        {
            return !(c is null) && !c.Disposed;
        }
        public static bool operator ==(ComponentBase c, object o)
        {
            if (o == null)
            {
                return !c;
            }
            return c.Equals(o);
        }
        public static bool operator !=(ComponentBase c, object o)
        {
            if (o == null)
            {
                return c;
            }
            return c.Equals(o);
        }

        /// <summary>
        /// Use this attribute to signal the engine to automatically assign the value of another component on this entity to this field
        /// </summary>
        [AttributeUsage(AttributeTargets.Field| AttributeTargets.Property)]
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
