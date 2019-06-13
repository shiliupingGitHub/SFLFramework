using System.Threading.Tasks;


namespace GGame.Core
{
    public class TcpService : IConService
    {
        public Task<bool> Connect(string ip, int port)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            return tcs.Task;
        }
    }
}