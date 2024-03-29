using System;
using System.Xml;
using GGame.Math;

namespace GGame.Core
{
#if Client_Logic
    [XLua.LuaCallCSharp]
#endif
    public class MoveComponent : Component, IXmlAwake
    {
        public Fix64 CurVSpeed = 0;

        public Fix64 Gravity
        {
            get;
            set;
        }
        public bool IsJump = false;

        public Fix64 HSpeed
        {
            get;
            set;
        }

        public Fix64 VSpeed
        {
            get;
            set;
        }

        public int JumpLandFrame
        {
            get;
            set;
        }

        public int CurJumpLandFrame = 0;
        public  void Awake(World world, XmlNode node)
        {


            Gravity = Convert.ToSingle(node.Attributes["g"].Value);
            HSpeed = Convert.ToSingle(node.Attributes["hSpeed"].Value);
            VSpeed = Convert.ToSingle(node.Attributes["vSpeed"].Value);
            JumpLandFrame = Convert.ToInt32(node.Attributes["jumpLandFrame"].Value);
        }
    }
}