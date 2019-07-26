using System;
using GGame.Math;
using UnityEngine;
using GGame.Core;
namespace GGame.Hybird
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

        public override FixVector3 Position
        {
            set
            {
                mGo.transform.position = new Vector3((float)value.x, (float) value.y, (float) value.z);
            }
        }

   
    }
}