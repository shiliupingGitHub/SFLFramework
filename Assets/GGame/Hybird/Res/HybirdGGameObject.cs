using System;
using GGame.Math;
using UnityEngine;

namespace GGame.Core
{
    public class HybirdGGameObject : GGameObject, IDisposable
    {
        private GameObject mGo;

        public GameObject GameObject
        {
            set { mGo = value; }
            get { return mGo; }

        }
        

        public void Dispose()
        {
            if(null != mGo)
                GameObject.Destroy(mGo);
            
            ObjectServer.Instance.Recycle(this);
        }

        public virtual FixVector3 Position
        {
            set { }
        }
    
    }
}