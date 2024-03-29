using Microsoft.Xna.Framework;
using VelcroPhysics.Dynamics.Joints;

namespace VelcroPhysics.Templates.Joints
{
    public class MotorJointTemplate : JointTemplate
    {
        public MotorJointTemplate() : base(JointType.Motor) { }

        /// <summary>
        /// The bodyB angle minus bodyA angle in radians.
        /// </summary>
        public GGame.Math.Fix64 AngularOffset { get; set; }

        /// <summary>
        /// Position correction factor in the range [0,1].
        /// </summary>
        public GGame.Math.Fix64 CorrectionFactor { get; set; }

        /// <summary>
        /// Position of bodyB minus the position of bodyA, in bodyA's frame, in meters.
        /// </summary>
        public Vector2 LinearOffset { get; set; }

        /// <summary>
        /// The maximum motor force in N.
        /// </summary>
        public GGame.Math.Fix64 MaxForce { get; set; }

        /// <summary>
        /// The maximum motor torque in N-m.
        /// </summary>
        public GGame.Math.Fix64 MaxTorque { get; set; }

        public override void SetDefaults()
        {
            MaxForce = 1.0f;
            MaxTorque = 1.0f;
            CorrectionFactor = 0.3f;
        }
    }
}