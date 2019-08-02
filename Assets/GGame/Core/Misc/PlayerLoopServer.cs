using System;

namespace GGame.Core
{
    public class PlayerLoopServer
    {
        protected event Action OnUpdate;
        protected event Action OnTick;
        private static PlayerLoopServer _instance;

        public static PlayerLoopServer Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new PlayerLoopServer();
                }

                return _instance;
            }
        }

        public PlayerLoopServer()
        {
            _instance = this;
        }

        public void AddUpdate(Action a)
        {
            OnUpdate += a;
        }

        public void RemoveUpdate(Action a)
        {
            OnUpdate -= a;
        }
        
        public void AddTick(Action a)
        {
            OnTick += a;
        }

        public void RemoveTick(Action a)
        {
            OnTick -= a;
        }

        protected void DoUpdate()
        {
            OnUpdate?.Invoke();
        }

        protected void DoTick()
        {
            OnTick?.Invoke();
        }
        
        
        
        
    }
}