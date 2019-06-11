
using GGame.Core;

namespace GGame.Core
{
    public class MoveCmdHandler : CmdHandler<World, Entity, MoveCmd>
    {
        
        protected override void Run(World world, Entity entity, MoveCmd a)
        {
            var mc = entity.GetComponent<MoveComponent>();
            
            mc.Dir = a.Dir;
        }
    }
}