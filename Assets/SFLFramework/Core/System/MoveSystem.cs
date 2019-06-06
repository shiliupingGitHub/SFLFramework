
using UnityEngine;

namespace GGame
{
    [Interest(typeof(MoveComponent))]
    public class MoveSystem : System
    {
        public override void OnUpdate()
        {
#if !SERVER
            foreach (MoveComponent mc in _interestComponents)
            {
                var rc = mc.Entity.GetComponent<RenderComponent>();

                if (null != rc)
                {
                    var pos = rc.Pos;
                    
                    UnityEngine.Vector3 gFinalPos = new UnityEngine.Vector3((float)pos.X, (float)pos.Y, (float)pos.Z);
                    var gPos = rc.GameObject.transform.position;
                    var dir = (gFinalPos - rc.GameObject.transform.position);
                    var len = dir.sqrMagnitude;
                    dir = dir.normalized;
                    float speed = (float) mc.Speed / 0.033f;
                    
                    if (mc.MoveLeftTime > 0.000001f)
                    {
                        float moveTime = Mathf.Min(mc.MoveLeftTime, UnityEngine.Time.deltaTime);
                        float moveLen = speed * moveTime;

                        moveLen = Mathf.Min(moveLen, len);

                        gFinalPos = gPos + dir * moveLen;

                        mc.MoveLeftTime = Mathf.Max(0, mc.MoveLeftTime - moveTime);
                        rc.Animator.SetFloat("SpeedX", 1.0f);
                    }
                    else
                    {

                        rc.Animator.SetFloat("SpeedX", 0.0f);
                    }
                    
                   

                    if (dir != UnityEngine.Vector3.zero)
                    {
                        rc.Animator.transform.forward = dir.normalized;
                        
                    }
                    
                    rc.GameObject.transform.position = gFinalPos;
                    

                }
                
            }
#endif
        }

        public override void OnTick()
        {
            foreach (MoveComponent mc in _interestComponents)
            {
                var rc = mc.Entity.GetComponent<RenderComponent>();

                if (null != rc)
                {
                    var pos = rc.Pos;

                    var dir = mc.Dir;
                    var speed = mc.Speed;
                    
                    pos += dir * speed;

                    rc.Pos = pos;
                    
                    if(dir.X != Fix64.Zero || dir.Y != Fix64.Zero || dir.Z != Fix64.Zero)
                        mc.MoveLeftTime = 0.033f;

                }
                
            }
        }
    }
}