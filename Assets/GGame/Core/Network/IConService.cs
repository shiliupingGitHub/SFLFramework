using System.Threading.Tasks;

namespace GGame.Core
{
    public interface IConService
    {
        Task<bool> Connect(string ip, int port);
    }
}