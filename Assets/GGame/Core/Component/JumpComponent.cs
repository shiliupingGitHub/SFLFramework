using System;
using System.Xml;
using GGame.Core;

namespace GGame.Core
{
    public class JumpComponent : Component
    {
        public Fix64 Speed { get; set; }
        public override void Awake(World world, XmlNode node)
        {
            base.Awake(world, node);
            Speed = (Fix64)Convert.ToSingle(node.Attributes?["speed"].Value);
            
        }
    }
}