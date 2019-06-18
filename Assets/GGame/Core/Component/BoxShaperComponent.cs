using System;
using System.Xml;
using GGame.Math;

namespace GGame.Core
{
    public class BoxShaperComponent : Component
    {
        public override void Awake(World world, XmlNode node)
        {
            base.Awake(world, node);

            Fix64 sizeX = Convert.ToSingle(node.Attributes["sizeX"].Value);
            Fix64 sizeY = Convert.ToSingle(node.Attributes["sizeY"].Value);
            Fix64 sizeZ = Convert.ToSingle(node.Attributes["sizeZ"].Value);
            
            Fix64 centerX = Convert.ToSingle(node.Attributes["centerX"].Value);
            Fix64 centerY = Convert.ToSingle(node.Attributes["centerY"].Value);
            Fix64 centerZ = Convert.ToSingle(node.Attributes["centerZ"].Value);
        }
    }
}