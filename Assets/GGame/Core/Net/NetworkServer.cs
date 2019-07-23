using NotImplementedException = System.NotImplementedException;

namespace GGame.Core
{
    public enum ConnectTye
    {
        TCP,
    }
    
    public class NetworkServer : IAutoInit
    {
        private static NetworkServer _instance;

        public static NetworkServer Instance
        {
            get
            {
                if(null == _instance)
                    _instance = new NetworkServer();
                return _instance;
            }
            
        }

        public NetworkServer()
        {
            _instance = this;
        }

        public  IChannel Create(ConnectTye type)
        {
            return null;
        }

        public void Init()
        {
            
        }
    }
}