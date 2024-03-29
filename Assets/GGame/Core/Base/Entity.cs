﻿using System;
using System.Collections.Generic;
using System.Xml;
using GGame.Math;

namespace GGame.Core
{
#if Client_Logic
    [XLua.LuaCallCSharp]
#endif
    public class Entity : IDisposable
    {
        public ulong PlayerId { get; set; } = 0;
        public ulong Id { get; set; }
        public FixVector3 Pos { get; set; }

        private World _world;

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
                Fix64 y = Convert.ToSingle( entityNode.Attributes?.GetNamedItem("positionY")?.Value);
                Fix64 z = Convert.ToSingle( entityNode.Attributes?.GetNamedItem("positionZ")?.Value);
                Pos = new FixVector3(x,y,z);
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
            PlayerId = 0;
            Id = 0;
            _world = null;
            Pos = FixVector3.Zero;
            foreach (var component in _components)
            {
                component.Value.Dispose();
            }
            
            _components.Clear();
        }
    }

}

