namespace GGame.Core
{
    public class LogServer
    {
        private static LogServer _instance;

        public static LogServer Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new LogServer();
                }

                return _instance;
            }
        }

        public LogServer()
        {
            _instance = this;
        }
        
        public virtual void Debug(string info)
        {

        }

        public virtual void Error(string info)
        {
        }
    }
}