

namespace GGame.Core
{
    public class JumpCmdHandler :  CmdHandler<World, Entity, JumpCmd>
    {
        
        protected override void Run(World world, Entity entity, JumpCmd a)
        {
            if(null == entity)
                return;
            
            var mc = entity.GetComponent<MoveComponent>();
            
            if(mc.IsJump)
                return;

            mc.CurVSpeed = 0.5f;
            mc.IsJump = true;
#if CLIENT_LOGIC
            var rc = entity.GetComponent<RenderComponent>();
            
            rc.Animator.SetTrigger("Jump");
            rc.Animator?.SetBool("IsJump", true);
#endif
        }
    }
}