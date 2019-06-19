using System;

namespace GGame.Core
{
    public class PlayerLoopManager
    {
        public Action OnUpdate;
        public Action OnTick;
        private static PlayerLoopManager _instance;

        public static PlayerLoopManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new PlayerLoopManager();
                }

                return _instance;
            }
        }

        public PlayerLoopManager()
        {
            _instance = this;
        }
        
        
        
    }
}