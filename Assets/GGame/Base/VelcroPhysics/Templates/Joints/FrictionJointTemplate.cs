using Microsoft.Xna.Framework;
using VelcroPhysics.Dynamics.Joints;

namespace VelcroPhysics.Templates.Joints
{
    public class FrictionJointTemplate : JointTemplate
    {
        public FrictionJointTemplate() : base(JointType.Friction) { }

        /// <summary>
        /// The local anchor point relative to bodyA's origin.
        /// </summary>
        public Vector2 LocalAnchorA { get; set; }

        /// <summary>
        /// The local anchor point relative to bodyB's origin.
        /// </summary>
        public Vector2 LocalAnchorB { get; set; }

        /// <summary>
        /// The maximum friction force in N.
        /// </summary>
        public GGame.Math.Fix64 MaxForce { get; set; }

        /// <summary>
        /// The maximum friction torque in N-m.
        /// </summary>
        public GGame.Math.Fix64 MaxTorque { get; set; }
    }
}