using System;

namespace GGame.Core
{
    public class PlayerLoopServer
    {
        public Action OnUpdate;
        public Action OnTick;
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
        
        
        
    }
}