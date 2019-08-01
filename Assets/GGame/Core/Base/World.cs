using System;
using System.Collections.Generic;
using System.Linq;
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
        Dictionary<ulong, GPlayer> _players = new Dictionary<ulong, GPlayer>();
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

        public void RemoveEntity(ulong id)
        {
            if (_entities.TryGetValue(id, out var e))
            {
                _entities.Remove(id);
                e.Dispose();
            }
                
           
        }

        public T CreatePlayer<T>(ulong uuid) where T: GPlayer, new()
        {
            var player = ObjectServer.Instance.Fetch<T>() ;
            
            player.Id = uuid;
            _players[uuid] = player;
            player.World = this;
            return player;
        }

        public void RemovePlayer(ulong uuid)
        {
            if (_players.TryGetValue(uuid, out var player))
            {
                _players.Remove(uuid);
                
                player.Dispose();
            }
            
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

        public Entity CreateEntity(int configId)
        {
            var configPath = $"entity_config_{configId}";
            var configText = GResourceServer.Instance.Load<string>(configPath) as string;
            var e = ObjectServer.Instance.Fetch<Entity>();
            var id = GeneratedUUIID;
            
            e.Init(this, configText);
            _entities[id] = e;
            e.Id = id;
            
            return e;
        }
        
        public Entity CreateEntityWithPos(int configId, Fix64 x, Fix64 z)
        {
            var configPath = $"entity_config_{configId}";
            var configText = GResourceServer.Instance.Load<string>(configPath) as string;
            var e = ObjectServer.Instance.Fetch<Entity>();
            var id = GeneratedUUIID;
            
            e.Pos = new FixVector2(x, z);
            e.Init(this, configText);
            _entities[id] = e;
            e.Id = id;
            
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

            foreach (var player in _players)
            {
                player.Value.Dispose();
            }
            _players.Clear();
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
                    
                    if (_players.TryGetValue(cmdInfo.Uuid, out var  player))
                    {
                        CmdServer.Instance.ExecuteCmd(this, player, cmdInfo.Cmd);
                    }
                    
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

