

namespace GGame.Core
{
    public class JumpCmdHandler :  CmdHandler<World, Entity, JumpCmd>
    {
        
        protected override void Run(World world, Entity entity, JumpCmd a)
        {
            if(null == entity)
                return;

        }
    }
}