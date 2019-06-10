
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

                    if (rc.Dir != FixVector3.Zero)
                    {
                        rc.Animator.SetFloat("SpeedX", 1.0f);
                        var dir = rc.Dir.GetNormalized();
                        var clientDir = new UnityEngine.Vector3((float)dir.x, (float)dir.y, (float)dir.z);
                        var speed = (float)rc.Speed / 0.04f;
                        var pos = rc.GameObject.transform.position + clientDir * speed * UnityEngine.Time.deltaTime;
                        rc.GameObject.transform.position = pos;
                        dir.y = Fix64.One;
                        var client_dir = new UnityEngine.Vector3((float)dir.x, (float)dir.y, (float)dir.z);

                        client_dir.y = 0;
                        rc.Animator.transform.forward = client_dir;
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


