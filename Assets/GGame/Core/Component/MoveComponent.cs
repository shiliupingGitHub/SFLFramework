using System;
using System.Xml;
using GGame.Math;

namespace GGame.Core
{
    public class MoveComponent : Component
    {
        public Fix64 CurVSpeed = 0;
        public Fix64 Gravity = 0.01f;
        public bool IsJump = false;
        public override void Awake(World world, XmlNode node)
        {
            base.Awake(world, node);
        }
    }
}