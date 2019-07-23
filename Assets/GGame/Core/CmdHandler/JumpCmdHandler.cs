

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

            mc.CurVSpeed = mc.VSpeed;
            mc.IsJump = true;
        }
    }
}