using GGame.Core;
using UnityEngine.SceneManagement;

namespace GGame.Hybird
{
    public class SceneManager : SingleTon<SceneManager>
    {
        public override void OnInit()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            
        }

        public void Load(string sceneName, System.Action onLoaded)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        
        
    }
}