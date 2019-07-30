using System.Threading.Tasks;
using NotImplementedException = System.NotImplementedException;

namespace GGame.Core
{
    public interface ITask
    {

        Task Run();
        
        Task<A> Run<A>();

    }


    public abstract class GTask : ITask
    {
        public Task Run()
        {
            return OnRun();
        }
        
        

        public Task<A> Run<A>()
        {
            throw new NotImplementedException();
        }

        protected abstract Task OnRun();
    }
    
    
    public abstract class GTask2 : ITask
    {
        public Task Run()
        {
            throw new NotImplementedException();
        }

        public Task<A> Run<A>()
        {
            return OnRun<A>();
        }
        
        protected abstract Task<A> OnRun<A>();
    }
    
    
}