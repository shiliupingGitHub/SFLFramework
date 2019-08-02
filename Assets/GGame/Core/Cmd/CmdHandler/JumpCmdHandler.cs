

namespace GGame.Core
{

    public class JumpCmdHandler :  CmdHandler<World, GPlayer, JumpCmd>
    {
#if Client_Logic
        [XLua.Hotfix]
#endif
        protected override void Run(World world, GPlayer player, JumpCmd a)
        {
           
        }
    }
}