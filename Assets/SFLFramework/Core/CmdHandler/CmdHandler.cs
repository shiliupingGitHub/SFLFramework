
using System;

namespace GGame
{
    public interface ICmdHandler
    {
        void Execute(Object a, Object b, Object o);
        void Execute(Object a, Object o);
        void Execute( Object o);
        Type Type { get; }
    }
    public abstract class CmdHandler<T, K, W> : ICmdHandler
    {
        protected abstract void Run(T world, K entity, W a);
        public void Execute(Object a,Object b, Object c)
        {
            Run((T)a, (K)b, (W)c);
        }

        public void Execute(object a, object o)
        {
            throw new NotImplementedException();
        }

        public void Execute(object o)
        {
            throw new NotImplementedException();
        }

        public Type Type {
            get { return typeof(W); }
            
        }
    }


    public abstract class CmdHandler<W> : ICmdHandler
    {
        public void Execute(object a, object b, object o)
        {
            throw new NotImplementedException();
        }

        public void Execute(object a, object o)
        {
            throw new NotImplementedException();
        }

        public void Execute(object o)
        {
            this.Run((W)o);
        }

        protected abstract void Run(W o);

        public Type Type {
            get { return typeof(W); }
            
        }
    }
    
    


} 


