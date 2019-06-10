using System;

namespace GGame
{
    public interface IProcedure
    {
        void Enter(Object o);
    }

    public abstract class Procedure<T> : IProcedure
    {
        public void Enter(object o)
        {
            this.OnEnter((T)o);
        }

        protected abstract void OnEnter(T o);
    }
}