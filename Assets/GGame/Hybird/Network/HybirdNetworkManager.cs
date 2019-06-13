using GGame.Core;

namespace GGame.Hybird
{
    [GGame.Core.AutoInit]
    public class HybirdNetworkManager : GGame.Core.NetworkManager
    {
        
        public override INetworkChannel CreateChannel(NETWORKD_TYPE type)
        {
            switch (type)
            {
                case NETWORKD_TYPE.TCP:
                    return new TcpNetworkChannel(); 
                default:
                    return null;
                    
            }
        }
    }
}