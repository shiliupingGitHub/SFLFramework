using System;
using System.Xml;
using GGame.Math;

namespace GGame.Core
{
    public class MoveComponent : Component
    {
        public bool IsLock = false;
        public Fix64 Speed { get; set; }
        public Fix64 Acceleration { get; set; }
        public FixVector3 Dir { get; set; } = FixVector3.Zero;
        public Fix64 MoveScale = (Fix64)0.0f;
        public Fix64 ConfigAcceleration { get; set; }
        public override void Awake(World world, XmlNode node)
        {
            base.Awake(world, node);

            Speed = (Fix64)Convert.ToSingle(node.Attributes?["speed"].Value);
            ConfigAcceleration = (Fix64)Convert.ToSingle(node.Attributes?["acceleration"].Value);
        }
    }
}