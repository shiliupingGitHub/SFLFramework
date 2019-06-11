using System;

namespace GGame.Support
{
    public interface IProcedure
    {
        void Enter();
        void Enter(Object o);
        void Enter(Object a, Object b);

    }

    public abstract class Procedure : IProcedure
    {
        public void Enter()
        {
            this.OnEnter();
        }

        public void Enter(object o)
        {
            throw new NotImplementedException();
        }

        public void Enter(object a, object b)
        {
            throw new NotImplementedException();
        }

        protected abstract void OnEnter();
    }
    public abstract class Procedure<T> : IProcedure
    {
        public void Enter()
        {
            throw new NotImplementedException();
        }

        public void Enter(object o)
        {
            this.OnEnter((T)o);
        }

        public void Enter(object a, object b)
        {
            throw new NotImplementedException();
        }

        protected abstract void OnEnter(T o);
    }

    public abstract class Procedure<T1, T2> : IProcedure
    {
        public void Enter()
        {
            throw new NotImplementedException();
        }

        public void Enter(object o)
        {
            throw new NotImplementedException();
        }

        public void Enter(object a, object b)
        {
            this.OnEnter((T1)a, (T2)b);
        }

        protected abstract void OnEnter(T1 a, T2 b);

    }
    
}