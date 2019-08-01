using System;
using System.Threading.Tasks;

namespace GGame.Core
{
    public class GResourceServer
    {
        private static GResourceServer _instance;

        public static GResourceServer Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new GResourceServer();
                }

                return _instance;
            }
        }

        public GResourceServer()
        {
            _instance = this;
        }

        public virtual object Load<T>(string path)
        {
            return null;
        }
    
        
    }
}