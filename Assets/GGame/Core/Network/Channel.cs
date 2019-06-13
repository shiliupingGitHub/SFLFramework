
using System.Threading.Tasks;

namespace GGame.Core
{
    public enum CONNECT_TYPE
    {
        TCP,
        UDP,
    }
    public class Channel
    {
        public IConService ConService { get; set; }
        public Channel(CONNECT_TYPE type)
        {
            switch (type)
            {
                case CONNECT_TYPE.TCP:
                    ConService = new TcpService();
                    break;
            }
        }

        public Task<bool> Connect(string ip, int port)
        {
            return ConService.Connect(ip, port);
        }
        
    }
}