
using UnityEngine;

namespace GGame
{
    [Interest(typeof(MoveComponent))]
    public class MoveSystem : System
    {
        public override void OnUpdate()
        {

        }

        public override void OnTick()
        {
            foreach (MoveComponent mc in _interestComponents)
            {
                var rc = mc.Entity.GetComponent<RenderComponent>();

                if (null != rc)
                {
                    if (!mc.IsLock)
                    {
                        var pos = rc.Pos;

                        var dir = mc.Dir;
                        var speed = mc.Speed;
                    
                        pos += dir * speed;

                        rc.Pos = pos;

                        rc.Speed = mc.Speed;
                        rc.Dir = dir;
                    }

                    else
                    {
                        rc.Dir = FixVector3.Zero;
                    }

                    
                }
                
            }
        }
    }
}