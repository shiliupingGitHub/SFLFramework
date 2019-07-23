using System;

namespace GGame.Core
{
    public class GGameObject : IDisposable
    {

        public virtual GAnimator GetAnimator()
        {
            return null;
        }


        public virtual void Dispose()
        {
            ObjectServer.Instance.Recycle(this);
        }
        
    }
}