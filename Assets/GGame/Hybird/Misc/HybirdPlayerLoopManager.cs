using GGame.Core;
using GGame.UnityCore;
using UnityEngine;

namespace GGame.Hybird
{
 
    public class HybirdPlayerLoopManager : PlayerLoopManager, IAutoInit
    {
        public void Init()
        {
            var go = new GameObject();
            
            GameObject.DontDestroyOnLoad(go);

            var looper =  go.AddComponent<Looper>();
            
            looper.LoopAction += delegate
            {
                if (null != OnUpdate)
                    OnUpdate();
            };
        }
    }
}