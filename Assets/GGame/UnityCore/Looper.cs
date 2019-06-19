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

        private void Start()
        {
            InvokeRepeating("OnTick", 0,0.033f);
        }

        void OnTick()
        {
            TickAction?.Invoke();
        }
    }
}

