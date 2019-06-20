
using GGame.Math;

namespace GGame.Core
{
    public class MoveCmdHandler : CmdHandler<World, Entity, MoveCmd>
    {
        
        protected override void Run(World world, Entity entity, MoveCmd a)
        {
            var mc = entity.GetComponent<MoveComponent>();


            if (a.isMove)
            {
                entity.MoveSpeedX = 1.0f;
                entity.Face = a.isLeft ? -1 : 1;
            }
            else
            {
                entity.MoveSpeedX = 0f;
            }
           
            
        }
    }
}