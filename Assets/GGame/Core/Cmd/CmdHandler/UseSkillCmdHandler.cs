

namespace GGame.Core
{
    public class UseSkillCmdHandler : CmdHandler<World, GPlayer, UseSkillCmd>
    {
#if Client_Logic
        [XLua.Hotfix]
#endif
        protected override void Run(World world, GPlayer player, UseSkillCmd a)
        {
            
           
        }
    }
}