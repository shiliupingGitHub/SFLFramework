
using System;
using System.Collections.Generic;
using System.Xml;

namespace GGame
{
    public class World : IDisposable
    {
        List<System> Systems = new List<System>();
        
        Dictionary<Type, List<System>> InterestSystems = new Dictionary<Type, List<System>>();
        
        Dictionary<ulong, Entity> _entities = new Dictionary<ulong, Entity>();
       
        public World()
        {
            Enverourment.Instance.CreateWorldSystem(this);
        }

        public void AddSystem(System system)
        {
            Systems.Add(system);
        }

        public Entity CreateEntity(ulong uuid, int configId)
        {
            var configPath = $"EntityConfig/entity_config_{configId}";
            var configText = ResourceManager.Instance.LoadEntityConfig(configPath);
            var e = new Entity(this, configText);
            _entities[uuid] = e;
            
            return e;
        }
        
        public void AddIntrest(Type t, System system)
        {
            List<System> ret = null;

            if (!InterestSystems.TryGetValue(t, out ret))
            {
                ret = new List<System>();
                InterestSystems[t] = ret;
            }
            
            ret.Add(system);
            
        }

        public List<System> GetInterstSystems(Type t)
        {
            List<System> ret = null;

            InterestSystems.TryGetValue(t, out ret);

            return ret;
        }
        
        
        public void Dispose()
        {
            foreach (var entity in _entities)
            {
                entity.Value.Dispose();
            }
            foreach (var system in Systems)
            {
                system.Dispose();
            }
            
            Systems.Clear();
        }

        public void Update()
        {
            foreach (var system in Systems)
            {
                system.OnUpdate();
            }
        }

        public void Tick()
        {
            foreach (var system in Systems)
            {
                system.OnTick();
            }
        }
    }
}

