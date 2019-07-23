using System;
using UnityEngine;

namespace GGame.Core
{
    public class HybirdGAnimator : GAnimator, IDisposable
    {
        public Animator Animator { get; set; }
        public void Dispose()
        {
            ObjectServer.Instance.Recycle(this);
        }
    }
}