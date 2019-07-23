
using GGame.Math;

namespace GGame.Core
{
    
    public  class TimeServer
    {
        
        private static TimeServer _instance;

        public static TimeServer Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new TimeServer();
                }

                return _instance;
            }
        }

        public TimeServer()
        {
            _instance = this;
        }
        
        public virtual Fix64 DeltaTime
        {
            get { return Fix64.Zero; }
        }
    }
}

