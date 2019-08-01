using System;
using System.Collections.Generic;
using System.Xml;
using GGame.Math;

namespace GGame.Core
{
    public class Entity : IDisposable
    {
        public ulong Id { get; set; }
        public FixVector2 Pos { get; set; }
        public Fix64 MoveSpeedX { get; set; }

        public int Face = 1;
        
        private World _world;
        public int Camp { get; set; }
        private readonly Dictionary<Type, Component> _components = new Dictionary<Type, Component>();
        public void Init(World world, string config)
        {
            
            XmlDocument document = new XmlDocument();
            
            document.LoadXml(config);
            
            var entityNode = document.FirstChild;
            
            Init(world, entityNode);
        }

        public void Init(World world, XmlNode entityNode)
        {
            if (null != entityNode.Attributes?.GetNamedItem("positionX"))
            {
                Fix64 x = Convert.ToSingle( entityNode.Attributes?.GetNamedItem("positionX")?.Value);
                Fix64 z = Convert.ToSingle( entityNode.Attributes?.GetNamedItem("positionZ")?.Value);
                Pos = new FixVector2(x,z);
            }
            
            this._world = world;
            var componentNode = entityNode.FirstChild;
            
            while (null != componentNode)
            {
                AddComponent(componentNode);
                componentNode = componentNode.NextSibling;
            }
        }
        
        public void AddComponent(XmlNode node)
        {
            var type = ComponentServer.Instance.GetComponentType(node.Name);

            if (null != type)
            {
                var component = ObjectServer.Instance.Fetch(type) as Component;
                var systems =  _world.GetInterstSystems(type);
            
                component.Entity = this;
                component.World = _world;
                
                
                if (null != systems)
                {
                    foreach (var system in systems)
                    {
                        system.AddInterest(component);
                    }
                }

                switch (component)
                {
                    case IXmlAwake xmlAwake:
                        xmlAwake.Awake(_world, node);
                        break;
                }

                _components[type] = component;
            }

            

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
            ObjectServer.Instance.Recycle(this);

            _world = null;
            foreach (var component in _components)
            {
                component.Value.Dispose();
            }
            
            _components.Clear();
        }
    }

}

