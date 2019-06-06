
namespace GGame
{
    public class MoveCmdHandler : CmdHandler<World, Entity, MoveCmd>
    {
        
        protected override void Run(World world, Entity entity, MoveCmd a)
        {
            var mc = entity.GetComponent<MoveComponent>();

            Vector3 dir = Vector3.Zero;
            dir.X =(Fix64) a.MoveX;
            dir.Y = (Fix64) a.MoveY;
            dir.Z = (Fix64) a.MoveZ;
            mc.Dir = dir;
        }
    }
}