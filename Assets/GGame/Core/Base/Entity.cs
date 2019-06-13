using System;
using System.Collections.Generic;
using System.Xml;

namespace GGame.Core
{
    public class Entity : IDisposable
    {
        private World _world;
        public int Camp { get; set; }
        private readonly Dictionary<Type, Component> _components = new Dictionary<Type, Component>();
        public void Init(World world, string config)
        {
            this._world = world;
            XmlDocument document = new XmlDocument();
            
            document.LoadXml(config);
            
            var entityNode = document.FirstChild;
            var componentNode = entityNode.FirstChild;
            
            while (null != componentNode)
            {
                AddComponent(componentNode);
                componentNode = componentNode.NextSibling;
            }
        }
        
        
        public void AddComponent(XmlNode node)
        {
            var type = CoreEnv.Instance.GetComponentType(node.Name);
            var component = ObjectPool.Instance.Fetch(type) as Component;
            
            component.Entity = this;
            component.Awake(this._world, node);
            _components[type] = component;
            

        }

        public T GetComponent<T>() where T:Component
        {
            var type = typeof(T);

            Component c = null;
            _components.TryGetValue(type, out c);

            return c as T;
        }
        
        public void Dispose()
        {
            ObjectPool.Instance.Recycle(this);

            _world = null;
            foreach (var component in _components)
            {
                component.Value.Dispose();
            }
            
            _components.Clear();
        }
    }

}

