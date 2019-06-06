using System;
using System.Xml;

namespace GGame
{
    public class MoveComponent : Component
    {
        public Fix64 Speed { get; set; }

        public Vector3 Dir { get; set; } = Vector3.Zero;
        
        #if !SERVER
        public float MoveLeftTime = 0;
        #endif
        public override void Awake(World world, XmlNode node)
        {
            base.Awake(world, node);

            Speed = (Fix64)Convert.ToSingle(node.Attributes?["speed"].Value);
        }
    }
}