using UnityEngine.SceneManagement;
using NotImplementedException = System.NotImplementedException;

namespace GGame.Support
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
        
        
    }
}