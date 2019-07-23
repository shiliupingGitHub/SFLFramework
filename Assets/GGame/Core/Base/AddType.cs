using System;

namespace GGame.Core
{
    public interface IAddType
    {

        void AddType(Type t);
    }

    public abstract class ServerAddType<T> : SingleTon<T>, IAddType  where T:new()
    {
        public virtual void AddType(Type t)
        {
            OnAdd(t);
        }
        
        public override  void OnInit(){}
        protected abstract void OnAdd(Type type);
    }
}