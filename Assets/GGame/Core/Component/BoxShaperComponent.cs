using System;
using System.Xml;
using GGame.Math;
using Jitter.Collision.Shapes;
using Jitter.Dynamics;
using Jitter.LinearMath;

namespace GGame.Core
{
    public class BoxShaperComponent : Component
    {
        public RigidBody RigidBody { get; set; }
        public override void Awake(World world, XmlNode node)
        {
            base.Awake(world, node);

            Fix64 sizeX = Convert.ToSingle(node.Attributes["sizeX"].Value);
            Fix64 sizeY = Convert.ToSingle(node.Attributes["sizeY"].Value);
            Fix64 sizeZ = Convert.ToSingle(node.Attributes["sizeZ"].Value);
            
            Fix64 centerX = Convert.ToSingle(node.Attributes["centerX"].Value);
            Fix64 centerY = Convert.ToSingle(node.Attributes["centerY"].Value);
            Fix64 centerZ = Convert.ToSingle(node.Attributes["centerZ"].Value);
            
            BoxShape shaper = new BoxShape(sizeX, sizeY, sizeZ);
            
            RigidBody = new RigidBody(shaper);
            
            RigidBody.Position = new JVector(Entity.Pos.X, Entity.Pos.Y , Entity.Pos.Z );
            RigidBody.Orientation = Entity.Orientation;
            world.GetSystem<PhysixSystem>().PhysixWorld?.AddBody(RigidBody);
            
        }
        

        public override void Dispose()
        {
            base.Dispose();
            
            if(null != RigidBody)
                _world.GetSystem<PhysixSystem>().PhysixWorld?.RemoveBody(RigidBody);
        }
    }
}