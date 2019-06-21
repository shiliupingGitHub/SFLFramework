using System;
using UnityEngine;

namespace GGame.UnityCore
{
    
    public class Looper : MonoBehaviour
    {
        public Action LoopAction;
        public Action TickAction;

        // Update is called once per frame
        void Update()
        {
            LoopAction?.Invoke();
        }
        
        public void StartTick(float time)
        {
            InvokeRepeating("OnTick", 0,time);
        }
        
        void OnTick()
        {
            TickAction?.Invoke();
        }
    }
}

