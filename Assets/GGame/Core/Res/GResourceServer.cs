namespace GGame.Core
{
    public class GResourceServer
    {
        private static GResourceServer _instance;

        public static GResourceServer Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new GResourceServer();
                }

                return _instance;
            }
        }

        public GResourceServer()
        {
            _instance = this;
        }
        
        public virtual string LoadText(string path)
        {

            return null;

        }
    
        public virtual byte[] LoadBytes(string path)
        {

            return null;

        }
    

        public virtual GGameObject LoadPrefab(string path)
        {
           
            return null;
        }

    }
}