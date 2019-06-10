﻿
using System;
using System.Collections.Generic;
using System.Xml;

namespace GGame
{
    public class World : IDisposable
    {
        private ulong _frameIndex = 0;
        readonly Dictionary<Type,System> _systems = new Dictionary<Type, System>();
        readonly Dictionary<Type, List<System>> _interestSystems = new Dictionary<Type, List<System>>();
        Dictionary<ulong, Entity> _entities = new Dictionary<ulong, Entity>();
        Dictionary<ulong, List<CmdInfo>> _cmdCache = new Dictionary<ulong, List<CmdInfo>>();
        List<IJob> _tickJobs = new List<IJob>();
        List<IJob> _CacheAddJob = new List<IJob>();
        List<IJob> _CacheRmoveJob = new List<IJob>();
        public ulong FrameIndex => _frameIndex;

        public World()
        {
            Enverourment.Instance.CreateWorldSystem(this);
        }

        public T GetSystem<T>() where T: System
        {
            System ret = null;
            var type = typeof(T);

            _systems.TryGetValue(type, out ret);

            return (T)ret;

        }
        public void AddCachCmde(ulong frameIndex, CmdInfo o)
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
            _systems[system.GetType()] = system;
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

            if (!_interestSystems.TryGetValue(t, out ret))
            {
                ret = new List<System>();
                _interestSystems[t] = ret;
            }
            
            ret.Add(system);
            
        }

        public List<System> GetInterstSystems(Type t)
        {
            List<System> ret = null;

            _interestSystems.TryGetValue(t, out ret);

            return ret;
        }
        
        
        public void Dispose()
        {
            foreach (var entity in _entities)
            {
                entity.Value.Dispose();
            }
            foreach (var system in _systems)
            {
                system.Value.Dispose();
            }
            _tickJobs.Clear();
            _interestSystems.Clear();
            _cmdCache.Clear();
            _systems.Clear();
            _frameIndex = 0;
            _CacheAddJob.Clear();
            _CacheRmoveJob.Clear();

        }

        public void Update()
        {
            foreach (var system in _systems)
            {
                system.Value.OnUpdate();
            }
        }

        public void AddTickJob(IJob job)
        {
            if(!_CacheAddJob.Contains(job))
            _CacheAddJob.Add(job);
        }

        public void RemvoeTickJob(Job job)
        {
            if(!_CacheRmoveJob.Contains(job))
            _CacheRmoveJob.Add(job);
        }
        

        public void Tick()
        {
            List<CmdInfo> cacheCmd = null;

            _cmdCache.TryGetValue(_frameIndex, out cacheCmd);

            if (null != cacheCmd)
            {
                foreach (var cmdInfo in cacheCmd)
                {
                    
                    Entity entity = null;
                    
                    _entities.TryGetValue(cmdInfo.Uuid, out entity);
                    Enverourment.Instance.ExecuteCmd(this, entity, cmdInfo.Cmd);
                }
            }
            
            foreach (var system in _systems)
            {
                system.Value.OnTick();
            }

            foreach (var job in _CacheRmoveJob)
            {
                _tickJobs.Remove(job);
            }
            _CacheRmoveJob.Clear();
            foreach (var job in _CacheAddJob)
            {
                if(!_tickJobs.Contains(job))
                    _tickJobs.Add(job);
            }
            _CacheAddJob.Clear();
            foreach (var job in _tickJobs)
            {
                job.Tick();
            }

            _frameIndex++;
        }
    }
}

