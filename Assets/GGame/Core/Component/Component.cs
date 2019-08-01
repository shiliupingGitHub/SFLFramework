using System;
using System.Xml;

namespace GGame.Core
{
    public abstract class Component : IDisposable
    {
        public World World;
        public Entity Entity
        {
            set;
            get;
        }
        
        public virtual void Dispose()
        {
            var type = GetType();
            var systems =  World.GetInterstSystems(type);

            if (null != systems)
            {
                foreach (var system in systems)
                {
                    system.RemoveInterest(this);
                }
            }

            World = null;
            Entity = null;
            
            ObjectServer.Instance.Recycle(this);

        }
        
    }

}

