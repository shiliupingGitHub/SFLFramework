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

            go.name = "Looper";
            go.hideFlags = HideFlags.HideInHierarchy;

            var looper =  go.AddComponent<Looper>();
            
            looper.LoopAction += delegate
            {
                OnUpdate?.Invoke();
            };
            
            looper.TickAction += delegate
            {
                OnTick?.Invoke();
            };
            
            looper.StartTick((float)ConstDefine.StepTime);
        }
        
        
    }
}