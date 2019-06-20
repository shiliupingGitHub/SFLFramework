using System;
using System.Collections.Generic;
using System.Xml;
using GGame.Math;

namespace GGame.Core
{
    public class Entity : IDisposable
    {
        public FixVector2 Pos { get; set; }
        public JVector Euler { get; set; } = new JVector(0, Fix64.PI * 0.5, 0);

        public int Face = 1;

        public JMatrix Orientation
        {
            get
            {
                var xMatrix = JMatrix.CreateRotationX(Euler.X);
                var yMatrix = JMatrix.CreateRotationY(Euler.Y);
                var zMatrix = JMatrix.CreateRotationZ(Euler.Z);

                return xMatrix * yMatrix * zMatrix;
            }
            
        }
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
                Fix64 y = Convert.ToSingle( entityNode.Attributes?.GetNamedItem("positionY")?.Value);
                Pos = new FixVector2(x, y);
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
            var type = GGameEnv.Instance.GetComponentType(node.Name);
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

