namespace GGame.Core
{
    public enum NETWORKD_TYPE
    {
        TCP,
    }
    
    public class NetworkManager
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

        public virtual INetworkChannel CreateChannel(NETWORKD_TYPE type)
        {
            return null;
        }
    }
}