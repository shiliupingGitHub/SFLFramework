
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

        }

        void UpdateGameObjectPos(RenderComponent rc, MoveComponent mc)
        {
            
#if CLIENT_LOGIC
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