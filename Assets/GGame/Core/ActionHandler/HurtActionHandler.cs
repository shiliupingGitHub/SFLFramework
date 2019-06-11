using GGame.Core;
using NotImplementedException = System.NotImplementedException;

namespace GGame.Core
{
    public class HurtActionHandler : CmdHandler<World, Entity, HurtAction>
    {
        protected override void Run(World world, Entity entity, HurtAction a)
        {
            Hurt h;
            h._Entity = entity;
            h._HurtAction = a;
            
            world.GetSystem<HurtedSystem>().AddHurt(h);
        }
    }
}