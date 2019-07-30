
namespace GGame.Core
{
    public interface IAutoInit
    {
        void Init();
    }

    public interface IAutoUpdate
    {
        void Update();
    }
    public abstract class SingleTon<T>  where T:new()
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new T();
                return _instance;
            }
        }
    }
}

