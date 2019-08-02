
using GGame.Math;

namespace GGame.Core
{
#if Client_Logic
    [XLua.LuaCallCSharp]
#endif
    [Cmd("move")]
    public struct MoveCmd
    {

        public bool isLeft;
        public bool isMove;
    }
}

