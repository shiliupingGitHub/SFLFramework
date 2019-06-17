
namespace GGame.Core
{
    public class MoveCmdHandler : CmdHandler<World, Entity, MoveCmd>
    {
        
        protected override void Run(World world, Entity entity, MoveCmd a)
        {
            var mc = entity.GetComponent<MoveComponent>();

            if (a.Dir == FixVector3.Zero)
            {
                mc.Acceleration = -mc.ConfigAcceleration;
                mc.MoveScale = (Fix64)1.0;
            }
            else
            {
                //mc.MoveScale = (Fix64)1.0f;
                mc.Dir = a.Dir;
                mc.Acceleration = mc.ConfigAcceleration;
            }
           
            
        }
    }
}