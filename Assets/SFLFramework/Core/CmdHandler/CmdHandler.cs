
using System;

namespace GGame
{
    public interface ICmdHandler
    {
        void Execute(World world, Entity entity, Object o);
        Type Type { get; }
    }
    public abstract class CmdHandler<T> : ICmdHandler
    {
        protected abstract void Run(World world, Entity entity, T a);
        public void Execute(World world,Entity entity, object o)
        {
            Run(world, entity, (T)o);
        }

        public Type Type {
            get { return typeof(T); }
            
        }
    }

} 


