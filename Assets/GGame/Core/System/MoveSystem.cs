
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
#if UNITY_2017_1_OR_NEWER  
            foreach (MoveComponent mc in _interestComponents)
            {
                var rc = mc.Entity.GetComponent<RenderComponent>();
                if (!mc.IsLock)
                {
                    var targetDis = mc.Speed * mc.MoveScale * 0.33;
                    var targetPos = mc.Entity.Pos + mc.Entity.Forward * targetDis;
                    
                    
                    var pos = mc.Entity.Pos;
                    var dir = mc.Entity.Forward;
                    Vector3 goPos = rc.GameObject.transform.position;

                    if (targetDis != Fix64.Zero)
                    {
                        var physix = World.GetSystem<PhysixSystem>().PhysixWorld;

                        if (physix.CollisionSystem.Raycast(pos, dir , null, out var body, out JVector n, out Fix64 f))
                        {
                            if (f > targetDis + mc.SizeX)
                            {
                                Fix64 dis = mc.Speed * mc.MoveScale * UnityEngine.Time.deltaTime;
                                
                                goPos += new UnityEngine.Vector3((float)dir.X, (float)dir.Y, (float)dir.Z) * (float)dis;
                            }
                            
                        }
                        else
                        {
                            Fix64 dis = mc.Speed * mc.MoveScale * UnityEngine.Time.deltaTime;
                                
                            goPos += new UnityEngine.Vector3((float)dir.X, (float)dir.Y, (float)dir.Z) * (float)dis;
                        }
                        

                        rc.GameObject.transform.position = goPos;

                    }
                }
                else
                {

                    if (null != rc)
                    {
                        if(null != rc.Animator)
                            rc.Animator.SetFloat("SpeedX", (float) 0);
                        rc.UpdatePostion();
                        rc.UpdateFace();
                    }
                }
            }
#endif
        }

        public override void OnTick()
        {

            foreach (MoveComponent mc in _interestComponents)
            {
                var rc = mc.Entity.GetComponent<RenderComponent>();
                
            
                if (!mc.IsLock)
                {
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
                    var dis = mc.Speed * mc.MoveScale * 0.033 ;
                    
                    var pos = mc.Entity.Pos;
                    var dir = mc.Entity.Forward;

                    if (dis != Fix64.Zero)
                    {
                        var physix = World.GetSystem<PhysixSystem>().PhysixWorld;

                        if (physix.CollisionSystem.Raycast(pos, dir , null, out var body, out JVector n, out Fix64 f))
                        {
                            if (f > dis + mc.SizeX)
                            {
                                pos += dir * dis;
                            }
                            
                        }
                        else
                        {
                            pos += dir * dis;
                        }
                    }
 
                    
                   
                    mc.Entity.Pos = pos;
#if UNITY_2017_1_OR_NEWER
                    if (null != rc)
                    {
                        if(null != rc.Animator)
                            rc.Animator.SetFloat("SpeedX", (float) mc.MoveScale);
                        rc.UpdatePostion();
                        rc.UpdateFace();
                    }
#endif
                }
                else
                {
#if UNITY_2017_1_OR_NEWER
                    if (null != rc)
                    {
                        if(null != rc.Animator)
                            rc.Animator.SetFloat("SpeedX", (float) 0);
                        rc.UpdatePostion();
                        rc.UpdateFace();
                    }
#endif
                }
                
                
            }
        }
    }
}