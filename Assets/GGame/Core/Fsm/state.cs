using NotImplementedException = System.NotImplementedException;

namespace GGame.Core
{
    public interface IState
    {
        void Enter();
        void Leave();

        void Update();
        
        void Enter(object a);

        void Enter(object a, object b);
    }

    public abstract class State : IState
    {
        public void Enter()
        {
            OnEnter();
        }

        public void Leave()
        {
            OnLeave();
        }

        public void Update()
        {
            OnUpdate();
        }

        public void Enter(object a)
        {
            throw new NotImplementedException();
        }

        public virtual void Enter(object a, object b)
        {
        }

        protected abstract void OnEnter();
        protected abstract void OnLeave();

        protected abstract void OnUpdate();

    }
    
    public abstract class State<A> : IState
    {
        
        public void Enter()
        {
        }

        public void Leave()
        {
            OnLeave();
        }

        public void Update()
        {
            OnUpdate();
        }

        public virtual void Enter(object a)
        {
            OnEnter((A)a);
        }

        public void Enter(object a, object b)
        {
            throw new NotImplementedException();
        }


        protected abstract void OnLeave();

        protected abstract void OnUpdate();
        
        protected abstract void OnEnter(A a);
      
    }

    public abstract class State<A, B> : IState
    {
        
        public void Enter()
        {
        }

        public void Leave()
        {
            OnLeave();
        }

        public void Update()
        {
            OnUpdate();
        }

        public void Enter(object a)
        {
            throw new NotImplementedException();
        }

        public virtual void Enter(object a, object b)
        {
            OnEnter((A)a, (B)b);
        }


        protected abstract void OnLeave();

        protected abstract void OnUpdate();
        
        protected abstract void OnEnter(A a, B b);
      
    }
}