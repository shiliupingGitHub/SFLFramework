using System;
using GGame.Math;
using UnityEngine;

namespace GGame.Core
{
    public class HybirdGGameObject : GGameObject
    {
        private GameObject mGo;

        public GameObject GameObject
        {
            set { mGo = value; }
            get { return mGo; }

        }
        

        public override void Dispose()
        {
            base.Dispose();
            if(null != mGo)
                GameObject.Destroy(mGo);
            
        }

        public override GAnimator GetAnimator()
        {
            if (null != mGo)
            {
                var ani = mGo.GetComponent<Animator>();

                if (null != ani)
                {
                    var gAni = ObjectServer.Instance.Fetch<HybirdGAnimator>();

                    gAni.Animator = ani;

                    return gAni;
                }
            }

            return null;
        }

        public virtual FixVector3 Position
        {
            set { }
        }

   
    }
}