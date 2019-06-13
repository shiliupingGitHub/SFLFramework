using GGame.Core;
using UnityEngine;

namespace GGame.Hybird
{
    public class StartUp
    {
        private static bool _isStartedUp = false;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
       static void OnStartUp()
        {
            if (!_isStartedUp)
            {
                GGameEnv.Instance.Init();
                Log.Debug("Start up");
            }
          
        }
    }
}