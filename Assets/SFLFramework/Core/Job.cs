using System;
using System.Collections.Generic;
using System.Xml;
using NotImplementedException = System.NotImplementedException;

namespace GGame
{
    public interface IJob : IDisposable
    {
        void Schedule(Action onFinish);
        void Init(World world, Entity entity, XmlNode data);
        void AddChild(IJob job);

        void Finish();

        void Tick();
    }

    public abstract class Job : IJob 
    {
        protected World _world;
        protected Entity _entity;
        
        List<IJob> _childs = new List<IJob>();
        protected Action mOnFinish;
        public void Schedule(Action onFinish)
        {
            _world?.AddTickJob(this);
           this.OnSchedule();
        }

        public void Finish()
        {
            _world?.RemvoeTickJob(this);
            if (_childs.Count > 0)
            {
                foreach (var child in _childs)
                {
                    child.Schedule(mOnFinish);
                }
            }
            else
            {
                if (null != mOnFinish)
                    mOnFinish();
            }
            

        }

        public  virtual  void Tick()
        {
            
        }
        public void Init(World world, Entity entity, XmlNode data)
        {
            _world = world;
            _entity = entity;
            
            this.OnInit(data);
        }

        public void AddChild(IJob job)
        {
            _childs.Add(job);
        }

        protected abstract void OnInit(XmlNode data);
        protected abstract void OnSchedule();
        public virtual void Dispose()
        {
            _world.RemvoeTickJob(this);
            _entity = null;
            _world = null;

            foreach (var job in _childs)
            {
                job.Dispose();
            }
            
            _childs.Clear();
            mOnFinish = null;
            ObjectPool.Instance.Recycle(this);
        }
    }
}