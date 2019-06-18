
using GGame.Math;

namespace GGame.Core
{
    public class MoveCmdHandler : CmdHandler<World, Entity, MoveCmd>
    {
        
        protected override void Run(World world, Entity entity, MoveCmd a)
        {
            var mc = entity.GetComponent<MoveComponent>();

            if (!a.isMove)
            {
                mc.Acceleration = -mc.ConfigAcceleration;
                //mc.MoveScale = (Fix64)1.0;
            }
            else
            {
                mc.MoveScale = 1.0f;
                entity.Euler = a.Dir;
                mc.Acceleration = mc.ConfigAcceleration;
            }
           
            
        }
    }
}