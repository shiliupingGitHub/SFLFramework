

namespace GGame.Core
{
    public class JumpCmdHandler :  CmdHandler<World, Entity, JumpCmd>
    {
        
        protected override void Run(World world, Entity entity, JumpCmd a)
        {
            if(null == entity)
                return;
            var mc = entity.GetComponent<MoveComponent>();
//            mc.IsLand = false;
//            mc.CurVSpeed = mc.VSpeed;
#if CLIENT_LOGIC
            var renerComponent = entity.GetComponent<RenderComponent>();
            renerComponent.Animator.SetTrigger("Jump");
            renerComponent.Animator.SetBool("IsLand", false);
#endif
        }
    }
}