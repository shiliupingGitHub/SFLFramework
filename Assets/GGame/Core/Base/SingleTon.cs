
namespace GGame.Core
{
    public interface IAutoInit
    {
        void Init();
    }
    public abstract class SingleTon<T>  where T:new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new T();
                return _instance;
            }
        }

        public  void Init()
        {
            OnInit();
        }

        public abstract void OnInit();
    }
}

