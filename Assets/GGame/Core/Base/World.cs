using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using GGame.Math;
using UnityEngine;

namespace GGame.Core
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
        List<IUpdate> _updates = new List<IUpdate>();
        List<ITick> _ticks = new List<ITick>();
        List<ILateUpate> _lateUpates = new List<ILateUpate>();
        public ulong FrameIndex => _frameIndex;
        private ulong incID = 1;
        private bool _isAutoTick = false;
        
        public World(bool autoTick)
        {
            SystemServer.Instance.Create(this);
            
            _isAutoTick = autoTick;
            if (autoTick)
            {
                PlayerLoopServer.Instance.OnUpdate += Update;
                PlayerLoopServer.Instance.OnTick += Tick;
            }
        }

        public ulong GeneratedUUIID
        {
            get
            {
                incID++;

                return incID;
            }
            
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
            
            system.World = this;
            _systems[system.GetType()] = system;

            if (system is IUpdate)
            {
                _updates.Add(system as IUpdate);
            }

            if (system is ITick)
            {
                _ticks.Add(system as ITick);
            }

            if (system is ILateUpate)
            {
                _lateUpates.Add(system as ILateUpate);
            }
            
            
        }

        public Entity CreateEntity(ulong uuid, int configId)
        {
            var configPath = $"entity_config_{configId}";
            var configText = GResourceServer.Instance.LoadText(configPath);
            var e = ObjectServer.Instance.Fetch<Entity>();
            
            e.Init(this, configText);
            _entities[uuid] = e;
            
            return e;
        }
        
        public Entity CreateEntityWithPos(ulong uuid, int configId, Fix64 x, Fix64 z)
        {
            var configPath = $"entity_config_{configId}";
            var configText = GResourceServer.Instance.LoadText(configPath);
            var e = ObjectServer.Instance.Fetch<Entity>();
            
            e.Pos = new FixVector2(x, z);
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
            _ticks.Clear();
            _updates.Clear();
            _lateUpates.Clear();
           
            if (_isAutoTick)
            {
                PlayerLoopServer.Instance.OnUpdate -= Update;
                PlayerLoopServer.Instance.OnTick -= Tick;
            }
            
            
        }

        public void Update()
        {
            foreach (var u in _updates)
            {
                u.Update();
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
                    CmdServer.Instance.ExecuteCmd(this, entity, cmdInfo.Cmd);
                }
            }
            
            foreach (var t in _ticks)
            {
                t.Tick();
            }
            
            foreach (var l in _lateUpates)
            {
                l.LateUpdate();
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

