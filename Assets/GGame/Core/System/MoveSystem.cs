
using GGame.Math;
using Jitter.LinearMath;
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
                var speed = mc.Speed * mc.MoveScale ;
                if (!mc.IsLock)
                {
                    
                    var pos = mc.Entity.Pos;
                    var dir = mc.Entity.Forward;

                    if (speed != Fix64.Zero)
                    {
                        var physix = World.GetSystem<PhysixSystem>().PhysixWorld;

                        if (physix.CollisionSystem.Raycast(pos, dir , null, out var body, out JVector n, out Fix64 f))
                        {
                            if (f > speed + 1)
                            {
                                pos += dir * speed;
                            }
                            
                        }
                        else
                        {
                            pos += dir * speed;
                        }
                    }
 
                    
                   
                    mc.Entity.Pos = pos;
                    
                    if(null != rc && null != rc.Animator)
                        rc.Animator.SetFloat("SpeedX", (float)mc.MoveScale);
                }
                
            }
        }
    }
}