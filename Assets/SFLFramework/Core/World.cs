
using System;
using System.Collections.Generic;
using System.Xml;

namespace GGame
{
    public class World : IDisposable
    {
        private ulong _frameIndex = 0;
        readonly List<System> Systems = new List<System>();
        readonly Dictionary<Type, List<System>> InterestSystems = new Dictionary<Type, List<System>>();
        Dictionary<ulong, Entity> _entities = new Dictionary<ulong, Entity>();
        Dictionary<ulong, List<CmdInfo>> _cmdCache = new Dictionary<ulong, List<CmdInfo>>();

        public ulong FrameIndex => _frameIndex;

        public World()
        {
            Enverourment.Instance.CreateWorldSystem(this);
        }

        public void AddCatchCmd(ulong frameIndex, CmdInfo o)
        {
            List<CmdInfo> ret = null;

            if (!_cmdCache.TryGetValue(frameIndex, out ret))
            {
                ret = new List<CmdInfo>();

                _cmdCache[frameIndex] = ret;
            }
            
            ret.Add(o);
        }
        

        public void AddSystem(System system)
        {
            Systems.Add(system);
        }

        public Entity CreateEntity(ulong uuid, int configId)
        {
            var configPath = $"EntityConfig/entity_config_{configId}";
            var configText = ResourceManager.Instance.LoadEntityConfig(configPath);
            var e = ObjectPool.Instance.Fetch<Entity>();
            
            e.Init(this, configText);
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
            List<CmdInfo> cacheCmd = null;

            _cmdCache.TryGetValue(_frameIndex, out cacheCmd);

            if (null != cacheCmd)
            {
                foreach (var cmdInfo in cacheCmd)
                {
                    var type = cmdInfo.Cmd .GetType();
                    var handler =  Enverourment.Instance.GetCmdHandler(type);

                    Entity entity = null;

                    _entities.TryGetValue(cmdInfo.Uuid, out entity);
                    handler.Execute(this, entity, cmdInfo.Cmd);
                }
            }
            
            foreach (var system in Systems)
            {
                system.OnTick();
            }

            _frameIndex++;
        }
    }
}

