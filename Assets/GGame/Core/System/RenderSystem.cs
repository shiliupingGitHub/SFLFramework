

using System;

namespace GGame.Core
{
    [Interest(typeof(RenderComponent))]
    public class RenderSystem : Core.System
    {
        public override void OnUpdate()
        {

           #if UNITY_2017_1_OR_NEWER
            foreach (RenderComponent rc in _interestComponents)
            {
                
                if (null != rc)
                {

                    if (rc.MoveDir != FixVector3.Zero)
                    {
                        var moveScale = UnityEngine.Mathf.Lerp((float)rc.MoveScale, (float)rc.MoveScale + (float)rc.Acceleration,
                            UnityEngine.Time.deltaTime);
                        rc.Animator.SetFloat("SpeedX", moveScale);
                        var dir = rc.MoveDir.GetNormalized();
                        var clientDir = new UnityEngine.Vector3((float)dir.x, (float)dir.y, (float)dir.z);
                        var speed = (float)rc.Speed / 0.04f * moveScale;
                        clientDir.Normalize();

                        if (!UnityEngine.Physics.Raycast(rc.GameObject.transform.position, clientDir,
                            speed * UnityEngine.Time.deltaTime + rc.Collider.radius, 1 << 0))
                        {
                            var pos = rc.GameObject.transform.position + clientDir * speed * UnityEngine.Time.deltaTime ;
                            rc.GameObject.transform.position = pos;
                            dir.y = Fix64.One;
                            var client_dir = new UnityEngine.Vector3((float)dir.x, (float)dir.y, (float)dir.z);

                            client_dir.y = 0;
                            rc.Animator.transform.forward = client_dir;
                        }
                    
                    }
                    else
                    {
                        var clinetPos = new UnityEngine.Vector3((float)rc.Pos.x, (float)rc.Pos.y, (float)rc.Pos.z);
                        rc.GameObject.transform.position = clinetPos;
                        rc.Animator.SetFloat("SpeedX", 0.0f);
                    }
                    

                }
                
            }
            #endif
        }

        public override void OnTick()
        {
           
        }
    }
}


