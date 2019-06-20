using System;
using System.Xml;
using GGame.Math;

namespace GGame.Core
{
    public class MoveComponent : Component
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
        public override void Awake(World world, XmlNode node)
        {
            base.Awake(world, node);

            Gravity = Convert.ToSingle(node.Attributes["g"].Value);
            HSpeed = Convert.ToSingle(node.Attributes["hSpeed"].Value);
            VSpeed = Convert.ToSingle(node.Attributes["vSpeed"].Value);
        }
    }
}