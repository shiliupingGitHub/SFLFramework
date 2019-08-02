
using GGame.Math;

namespace GGame.Core
{
    
    public class MoveCmdHandler : CmdHandler<World, GPlayer, MoveCmd>
    {
#if Client_Logic
        [XLua.Hotfix]
#endif
        protected override void Run(World world, GPlayer player, MoveCmd a)
        {

            int k = 0;
            int j = k;

        }
    }
}