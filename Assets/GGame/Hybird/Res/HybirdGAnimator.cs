using System;
using UnityEngine;

namespace GGame.Core
{
    public class HybirdGAnimator : GAnimator, IDisposable
    {
        public Animator Animator { get; set; }
        public override void Dispose()
        {
            base.Dispose();

            Animator = null;
        }
    }
}