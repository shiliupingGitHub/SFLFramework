
namespace GGame
{
    public class MoveCmdHandler : CmdHandler<MoveCmd>
    {
        protected override void Run(World world,Entity entity, MoveCmd a)
        {
            var mc = entity.GetComponent<MoveComponent>();

            Vector3 dir = Vector3.Zero;
            dir.X =(Fix64) a.Dir;
            mc.Dir = dir;
        }
    }
}