using System;
using System.Collections.Generic;
using System.Xml;

namespace GGame
{
    public abstract class Component : IDisposable
    {
        protected World _world;
        public Entity Entity
        {
            set;
            get;
        }

        public virtual void Dispose()
        {
            var type = GetType();
            var systems =  _world.GetInterstSystems(type);

            foreach (var system in systems)
            {
                system.RemoveInterest(this);
            }
            
        }

        public virtual void Awake(World world, XmlNode node)
        {
            this._world = world;
            var type = GetType();
            var systems =  world.GetInterstSystems(type);

            foreach (var system in systems)
            {
                system.AddInterest(this);
            }

        }
    }

}

