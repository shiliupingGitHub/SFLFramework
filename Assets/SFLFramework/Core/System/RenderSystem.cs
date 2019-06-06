
using System;

namespace GGame
{
    [Interest(typeof(RenderComponent))]
    public class RenderSystem : System
    {
        public override void OnUpdate()
        {

           #if !SERVER
            foreach (RenderComponent rc in _interestComponents)
            {
                
                if (null != rc)
                {
                    var pos = rc.Pos;
                    
                    UnityEngine.Vector3 gFinalPos = new UnityEngine.Vector3((float)pos.X, (float)pos.Y, (float)pos.Z);
                    var gPos = rc.GameObject.transform.position;
                    var dir = (gFinalPos - rc.GameObject.transform.position);
                    var len = dir.sqrMagnitude;
                    dir = dir.normalized;
                 
                    
                    if (rc.MoveLeftTime > 0f)
                    {
                        float speed = rc.Speed / 0.04f;
                        float moveTime = UnityEngine. Mathf.Min(rc.MoveLeftTime, UnityEngine.Time.deltaTime);
                        float moveLen = speed * moveTime;

                        moveLen = UnityEngine. Mathf.Min(moveLen, len);

                        gFinalPos = gPos + dir * moveLen;

                        rc.MoveLeftTime = UnityEngine.Mathf.Max(0, rc.MoveLeftTime - moveTime);
                        rc.Animator.SetFloat("SpeedX", 1.0f);
                        dir.y = 0;
                        rc.Animator.transform.forward = dir;
                    }
                    else
                    {

                        rc.Animator.SetFloat("SpeedX", 0.0f);
                        rc.UpdatePostion();
                    }
                    
                    
                    rc.GameObject.transform.position = gFinalPos;
                    

                }
                
            }
            #endif
        }

        public override void OnTick()
        {
           
        }
    }
}


