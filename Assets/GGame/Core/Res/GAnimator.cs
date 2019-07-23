using System;

namespace GGame.Core
{
    public class GAnimator : IDisposable
    {
        public virtual void Dispose()
        {
            ObjectServer.Instance.Recycle(this);
        }
    }
}