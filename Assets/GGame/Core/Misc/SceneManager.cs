namespace GGame.Core
{
    public class SceneManager
    {
        private static SceneManager _instance;

        public static SceneManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new SceneManager();
                }

                return _instance;
            }
        }

        public SceneManager()
        {
            _instance = this;
        }
    }
}