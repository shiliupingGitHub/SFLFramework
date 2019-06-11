using UnityEngine;
using GGame;
namespace GGame.Support
{
    public class StartUp
    {
        private static bool _isStartedUp = false;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
       static void OnStartUp()
        {
            if (!_isStartedUp)
            {
                WorldEnv.Instance.Init();
                SupportEnv.Instance.Init();
                Log.Debug("Start up");
            }
          
        }
    }
}