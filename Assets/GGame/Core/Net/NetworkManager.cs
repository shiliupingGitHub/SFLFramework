using NotImplementedException = System.NotImplementedException;

namespace GGame.Core
{
    public enum ConnectTye
    {
        TCP,
    }
    
    public class NetworkManager : IAutoInit
    {
        private static NetworkManager _instance;

        public static NetworkManager Instance
        {
            get
            {
                if(null == _instance)
                    _instance = new NetworkManager();
                return _instance;
            }
            
        }

        public NetworkManager()
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