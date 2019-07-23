using UnityEngine.SceneManagement;

namespace GGame.Core
{
    public class SceneServer
    {
        private static SceneServer _instance;

        public static SceneServer Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new SceneServer();
                }

                return _instance;
            }
        }

        public SceneServer()
        {
            _instance = this;
        }
    }
}