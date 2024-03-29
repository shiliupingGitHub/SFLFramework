using GGame.Core;
using GGame.UnityCore;
using UnityEngine;

namespace GGame.Hybird
{
 
    public class HybirdPlayerLoopServer : PlayerLoopServer, IAutoInit
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
                DoUpdate();
            };
            
            looper.TickAction += delegate
            {
               DoTick();
            };
            
            looper.StartTick((float)ConstDefine.StepTime);
        }
        
        
    }
}