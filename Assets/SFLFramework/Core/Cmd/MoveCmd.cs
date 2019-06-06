
namespace GGame
{
    [Cmd("move")]
    public struct MoveCmd
    {

        public int MoveX;
        public int MoveY;
        public int MoveZ;

        public MoveCmd(int x, int y, int z)
        {
            MoveX = x;
            MoveY = y;
            MoveZ = z;
        }
        
        public static MoveCmd Zero()
        {
            return new MoveCmd(0, 0, 0);
        }

    }
}

