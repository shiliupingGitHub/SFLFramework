
using GGame.Math;
using Jitter.LinearMath;

namespace GGame.Core
{
    [Cmd("move")]
    public struct MoveCmd
    {

        public JVector Dir;
        public bool isMove;
    }
}

