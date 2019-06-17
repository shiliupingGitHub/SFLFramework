
using UnityEngine;

namespace GGame.Core
{
    [Interest(typeof(MoveComponent))]
    public class MoveSystem : Core.System
    {
        public override void OnUpdate()
        {

        }

        public override void OnTick()
        {
#if UNITY_2017_1_OR_NEWER  
            foreach (MoveComponent mc in _interestComponents)
            {
                var rc = mc.Entity.GetComponent<RenderComponent>();
                mc.MoveScale += mc.Acceleration;
                if (mc.MoveScale <= Fix64.Zero)
                {
                    mc.MoveScale = Fix64.Zero;
                    mc.Acceleration = Fix64.Zero;
                }

                if (mc.MoveScale >= Fix64.One)
                {
                    mc.MoveScale = Fix64.One;
                    mc.Acceleration = Fix64.Zero;
                }
                
                
                if (null != rc && rc.Collider != null)
                {
                    if (!mc.IsLock)
                    {
                        
                        
                        
                        var pos = rc.Pos;

                        var dir = mc.Dir;
                        var speed = mc.Speed * mc.MoveScale;

                        if (!UnityEngine.Physics.Raycast(new Vector3((float) pos.x, (float) pos.y, (float) pos.z),
                            new Vector3((float) dir.x, (float) dir.y, (float) pos.z)
                            , (float) speed + rc.Collider.radius, 1 << 0))
                        {
                            pos += dir * speed;
                        }
                        

                       
                        
                        
                        rc.Pos = pos;

                        rc.Speed = mc.Speed;
                        rc.MoveDir = dir;
                        rc.MoveScale = mc.MoveScale;
                        rc.Acceleration = mc.Acceleration;
                        dir.y = Fix64.Zero;
                        
                        if(dir != FixVector3.Zero)
                            rc.Face = dir.GetNormalized();
                    }

                    else
                    {
                        rc.MoveDir = FixVector3.Zero;
                    }

                    
                }
                
            }
#endif
        }
    }
}