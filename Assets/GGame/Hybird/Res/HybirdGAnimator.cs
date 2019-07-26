using System;
using UnityEngine;
using GGame.Core;
namespace GGame.Hybird
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