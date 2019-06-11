using System;
using UnityEngine;

namespace GGame.UnityCore
{
    
    public class Looper : MonoBehaviour
    {
        public Action LoopAction;

        // Update is called once per frame
        void Update()
        {
            LoopAction?.Invoke();
        }
    }
}

