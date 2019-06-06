using System;
using System.Xml;

namespace GGame
{
    public class GJumpComponent : Component
    {
        public Fix64 Speed { get; set; }
        public override void Awake(World world, XmlNode node)
        {
            base.Awake(world, node);
            Speed = (Fix64)Convert.ToSingle(node.Attributes?["speed"].Value);
            
        }
    }
}