using System;
using System.Collections.Generic;
using System.Xml;
using GGame.Math;
using Jitter.LinearMath;

namespace GGame.Core
{
    public class Entity : IDisposable
    {
        public JVector Pos { get; set; }
        public JVector Euler { get; set; } = new JVector(0, Fix64.PI * 0.5, 0);

        public JVector Forward
        {
            get
            {
                
               var xMatrix = JMatrix.CreateRotationX(Euler.X);
               var yMatrix = JMatrix.CreateRotationY(Euler.Y);
               var zMatrix = JMatrix.CreateRotationZ(Euler.Z);

               var v0 = JVector.Transform(JVector.Forward, xMatrix);
               var v1 = JVector.Transform(v0, yMatrix);
               var v2 = JVector.Transform(v1, zMatrix);
               v2.Z = 0;
               v2.Y = 0;
               
               if(v2 != JVector.Zero)
                    v2.Normalize();
               return v2;
            }
        }

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
            Fix64 x = Convert.ToSingle( entityNode.Attributes?.GetNamedItem("positionX")?.Value);
            Fix64 y = Convert.ToSingle( entityNode.Attributes?.GetNamedItem("positionY")?.Value);
            Fix64 z = Convert.ToSingle( entityNode.Attributes?.GetNamedItem("positionZ")?.Value);
            
            Fix64 eulerX = Convert.ToSingle( entityNode.Attributes?.GetNamedItem("eulerX")?.Value);
            Fix64 eulerY = Convert.ToSingle( entityNode.Attributes?.GetNamedItem("eulerY")?.Value);
            Fix64 eulerZ = Convert.ToSingle( entityNode.Attributes?.GetNamedItem("eulerZ")?.Value);
            
            Pos = new JVector(x, y, z);
            Euler = new JVector(eulerX, eulerY, eulerZ);
            
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

