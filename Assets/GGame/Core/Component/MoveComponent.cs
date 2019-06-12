using System;
using System.Xml;

namespace GGame.Core
{
    public class MoveComponent : Component
    {
        public bool IsLock = false;
        public Fix64 Speed { get; set; }

        public FixVector3 Dir { get; set; } = FixVector3.Zero;
        public override void Awake(World world, XmlNode node)
        {
            base.Awake(world, node);

            Speed = (Fix64)Convert.ToSingle(node.Attributes?["speed"].Value);
        }
    }
}