
namespace GGame.Core
{
#if Client_Logic
    [XLua.LuaCallCSharp]
#endif
    [Cmd("skill")]
    public struct UseSkillCmd
    {
        public int id;
    }
}