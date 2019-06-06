
using System;

namespace GGame
{
    public interface ICmdHandler
    {
        void Execute(Object a, Object b, Object o);
        Type Type { get; }
    }
    public abstract class CmdHandler<T, K, W> : ICmdHandler
    {
        protected abstract void Run(T world, K entity, W a);
        public void Execute(Object a,Object b, Object c)
        {
            Run((T)a, (K)b, (W)c);
        }

        public Type Type {
            get { return typeof(W); }
            
        }
    }

} 


