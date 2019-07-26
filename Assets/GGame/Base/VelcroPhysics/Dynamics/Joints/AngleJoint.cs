/*
* Velcro Physics:
* Copyright (c) 2017 Ian Qvist
*/

using System;
using System.Diagnostics;
using GGame.Math;
using Microsoft.Xna.Framework;
using VelcroPhysics.Dynamics.Solver;

namespace VelcroPhysics.Dynamics.Joints
{
    /// <summary>
    /// Maintains a fixed angle between two bodies
    /// </summary>
    public class AngleJoint : Joint
    {
        private GGame.Math.Fix64 _bias;
        private GGame.Math.Fix64 _jointError;
        private GGame.Math.Fix64 _massFactor;
        private GGame.Math.Fix64 _targetAngle;

        internal AngleJoint()
        {
            JointType = JointType.Angle;
        }

        /// <summary>
        /// Constructor for AngleJoint
        /// </summary>
        /// <param name="bodyA">The first body</param>
        /// <param name="bodyB">The second body</param>
        public AngleJoint(Body bodyA, Body bodyB)
            : base(bodyA, bodyB)
        {
            JointType = JointType.Angle;
            BiasFactor = .2f;
            MaxImpulse = GGame.Math.Fix64.MaxValue;
        }

        public override Vector2 WorldAnchorA
        {
            get { return BodyA.Position; }
            set { Debug.Assert(false, "You can't set the world anchor on this joint type."); }
        }

        public override Vector2 WorldAnchorB
        {
            get { return BodyB.Position; }
            set { Debug.Assert(false, "You can't set the world anchor on this joint type."); }
        }

        /// <summary>
        /// The desired angle between BodyA and BodyB
        /// </summary>
        public GGame.Math.Fix64 TargetAngle
        {
            get { return _targetAngle; }
            set
            {
                if (value != _targetAngle)
                {
                    _targetAngle = value;
                    WakeBodies();
                }
            }
        }

        /// <summary>
        /// Gets or sets the bias factor.
        /// Defaults to 0.2
        /// </summary>
        public GGame.Math.Fix64 BiasFactor { get; set; }

        /// <summary>
        /// Gets or sets the maximum impulse
        /// Defaults to GGame.Math.Fix64.MaxValue
        /// </summary>
        public GGame.Math.Fix64 MaxImpulse { get; set; }

        /// <summary>
        /// Gets or sets the softness of the joint
        /// Defaults to 0
        /// </summary>
        public GGame.Math.Fix64 Softness { get; set; }

        public override Vector2 GetReactionForce(GGame.Math.Fix64 invDt)
        {
            //TODO
            //return _inv_dt * _impulse;
            return Vector2.Zero;
        }

        public override GGame.Math.Fix64 GetReactionTorque(GGame.Math.Fix64 invDt)
        {
            return 0;
        }

        internal override void InitVelocityConstraints(ref SolverData data)
        {
            int indexA = BodyA.IslandIndex;
            int indexB = BodyB.IslandIndex;

            GGame.Math.Fix64 aW = data.Positions[indexA].A;
            GGame.Math.Fix64 bW = data.Positions[indexB].A;

            _jointError = (bW - aW - TargetAngle);
            _bias = -BiasFactor * data.Step.inv_dt * _jointError;
            _massFactor = (1 - Softness) / (BodyA._invI + BodyB._invI);
        }

        internal override void SolveVelocityConstraints(ref SolverData data)
        {
            int indexA = BodyA.IslandIndex;
            int indexB = BodyB.IslandIndex;

            GGame.Math.Fix64 p = (_bias - data.Velocities[indexB].W + data.Velocities[indexA].W) * _massFactor;

            data.Velocities[indexA].W -= BodyA._invI * Fix64.Sign(p) * Math.Min((float)Fix64.Abs(p), (float)MaxImpulse);
            data.Velocities[indexB].W += BodyB._invI * Fix64.Sign(p) * Math.Min((float)Fix64.Abs(p), (float)MaxImpulse);
        }

        internal override bool SolvePositionConstraints(ref SolverData data)
        {
            //no position solving for this joint
            return true;
        }
    }
}