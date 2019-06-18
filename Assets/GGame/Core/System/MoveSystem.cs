
using GGame.Math;
using Jitter.LinearMath;

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
            
                if (!mc.IsLock)
                {
                   
                   
                    var dis = mc.Speed * mc.MoveScale  ;
                    
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
                        
                        mc.Entity.Pos = pos;
                        
                    }

                }
                UpdateGameObjectPos(rc, mc);
               
                
                
            }
        }

        void UpdateGameObjectPos(RenderComponent rc, MoveComponent mc)
        {
            
#if UNITY_2017_1_OR_NEWER
            if (null != rc)
            {
                if(null != rc.Animator)
                    rc.Animator.SetFloat("SpeedX", (float) mc.MoveScale);

                if (null != rc.GameObject)
                {
                    UnityEngine.Vector3 unityPos = new UnityEngine.Vector3((float)mc.Entity.Pos.X, (float)mc.Entity.Pos.Y, (float)mc.Entity.Pos.Z);
                    UnityEngine.Vector3 goPos = rc.GameObject.transform.position;

                    float d = UnityEngine.Vector3.Distance(unityPos, goPos);

                    rc.UpdatePostion();
                    rc.UpdateFace();
                            
                }
                        
            }
#endif
        }
    }
}