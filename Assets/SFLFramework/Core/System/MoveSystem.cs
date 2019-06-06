
using UnityEngine;

namespace GGame
{
    [Interest(typeof(MoveComponent))]
    public class MoveSystem : System
    {
        public override void OnUpdate()
        {
#if !SERVER
            
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
#if !SERVER
                    if (dir.X != Fix64.Zero || dir.Y != Fix64.Zero || dir.Z != Fix64.Zero)
                    {
                        rc.MoveLeftTime = 0.04f;
                        rc.Speed =(float) mc.Speed;

                    }
                    
#endif
    
                  

                }
                
            }
        }
    }
}