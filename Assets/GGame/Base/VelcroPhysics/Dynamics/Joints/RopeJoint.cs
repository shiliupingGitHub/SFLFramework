﻿/*
* Velcro Physics:
* Copyright (c) 2017 Ian Qvist
* 
* Original source Box2D:
* Copyright (c) 2006-2011 Erin Catto http://www.box2d.org 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

using System;
using Microsoft.Xna.Framework;
using VelcroPhysics.Dynamics.Solver;
using VelcroPhysics.Shared;
using VelcroPhysics.Utilities;

namespace VelcroPhysics.Dynamics.Joints
{
    // Limit:
    // C = norm(pB - pA) - L
    // u = (pB - pA) / norm(pB - pA)
    // Cdot = dot(u, vB + cross(wB, rB) - vA - cross(wA, rA))
    // J = [-u -cross(rA, u) u cross(rB, u)]
    // K = J * invM * JT
    //   = invMassA + invIA * cross(rA, u)^2 + invMassB + invIB * cross(rB, u)^2

    /// <summary>
    /// A rope joint enforces a maximum distance between two points on two bodies. It has no other effect.
    /// It can be used on ropes that are made up of several connected bodies, and if there is a need to support a heavy body.
    /// This joint is used for stabilization of heavy objects on soft constraint joints.
    /// Warning: if you attempt to change the maximum length during the simulation you will get some non-physical behavior.
    /// Use the DistanceJoint instead if you want to dynamically control the length.
    /// </summary>
    public class RopeJoint : Joint
    {
        // Solver shared
        private GGame.Math.Fix64 _impulse;

        // Solver temp
        private int _indexA;

        private int _indexB;
        private GGame.Math.Fix64 _invIA;
        private GGame.Math.Fix64 _invIB;
        private GGame.Math.Fix64 _invMassA;
        private GGame.Math.Fix64 _invMassB;
        private GGame.Math.Fix64 _length;
        private Vector2 _localCenterA;
        private Vector2 _localCenterB;
        private GGame.Math.Fix64 _mass;
        private Vector2 _rA, _rB;
        private Vector2 _u;

        internal RopeJoint()
        {
            JointType = JointType.Rope;
        }

        /// <summary>
        /// Constructor for RopeJoint.
        /// </summary>
        /// <param name="bodyA">The first body</param>
        /// <param name="bodyB">The second body</param>
        /// <param name="anchorA">The anchor on the first body</param>
        /// <param name="anchorB">The anchor on the second body</param>
        /// <param name="useWorldCoordinates">Set to true if you are using world coordinates as anchors.</param>
        public RopeJoint(Body bodyA, Body bodyB, Vector2 anchorA, Vector2 anchorB, bool useWorldCoordinates = false)
            : base(bodyA, bodyB)
        {
            JointType = JointType.Rope;

            if (useWorldCoordinates)
            {
                LocalAnchorA = bodyA.GetLocalPoint(anchorA);
                LocalAnchorB = bodyB.GetLocalPoint(anchorB);
            }
            else
            {
                LocalAnchorA = anchorA;
                LocalAnchorB = anchorB;
            }

            //Velcro feature: Setting default MaxLength
            Vector2 d = WorldAnchorB - WorldAnchorA;
            MaxLength = d.Length();
        }

        /// <summary>
        /// The local anchor point on BodyA
        /// </summary>
        public Vector2 LocalAnchorA { get; set; }

        /// <summary>
        /// The local anchor point on BodyB
        /// </summary>
        public Vector2 LocalAnchorB { get; set; }

        public sealed override Vector2 WorldAnchorA
        {
            get { return BodyA.GetWorldPoint(LocalAnchorA); }
            set { LocalAnchorA = BodyA.GetLocalPoint(value); }
        }

        public sealed override Vector2 WorldAnchorB
        {
            get { return BodyB.GetWorldPoint(LocalAnchorB); }
            set { LocalAnchorB = BodyB.GetLocalPoint(value); }
        }

        /// <summary>
        /// Get or set the maximum length of the rope.
        /// By default, it is the distance between the two anchor points.
        /// </summary>
        public GGame.Math.Fix64 MaxLength { get; set; }

        /// <summary>
        /// Gets the state of the joint.
        /// </summary>
        public LimitState State { get; private set; }

        public override Vector2 GetReactionForce(GGame.Math.Fix64 invDt)
        {
            return (invDt * _impulse) * _u;
        }

        public override GGame.Math.Fix64 GetReactionTorque(GGame.Math.Fix64 invDt)
        {
            return 0;
        }

        internal override void InitVelocityConstraints(ref SolverData data)
        {
            _indexA = BodyA.IslandIndex;
            _indexB = BodyB.IslandIndex;
            _localCenterA = BodyA._sweep.LocalCenter;
            _localCenterB = BodyB._sweep.LocalCenter;
            _invMassA = BodyA._invMass;
            _invMassB = BodyB._invMass;
            _invIA = BodyA._invI;
            _invIB = BodyB._invI;

            Vector2 cA = data.Positions[_indexA].C;
            GGame.Math.Fix64 aA = data.Positions[_indexA].A;
            Vector2 vA = data.Velocities[_indexA].V;
            GGame.Math.Fix64 wA = data.Velocities[_indexA].W;

            Vector2 cB = data.Positions[_indexB].C;
            GGame.Math.Fix64 aB = data.Positions[_indexB].A;
            Vector2 vB = data.Velocities[_indexB].V;
            GGame.Math.Fix64 wB = data.Velocities[_indexB].W;

            Rot qA = new Rot(aA), qB = new Rot(aB);

            _rA = MathUtils.Mul(qA, LocalAnchorA - _localCenterA);
            _rB = MathUtils.Mul(qB, LocalAnchorB - _localCenterB);
            _u = cB + _rB - cA - _rA;

            _length = _u.Length();

            GGame.Math.Fix64 C = _length - MaxLength;
            if (C > 0.0f)
            {
                State = LimitState.AtUpper;
            }
            else
            {
                State = LimitState.Inactive;
            }

            if (_length > Settings.LinearSlop)
            {
                _u *= 1.0f / _length;
            }
            else
            {
                _u = Vector2.Zero;
                _mass = 0.0f;
                _impulse = 0.0f;
                return;
            }

            // Compute effective mass.
            GGame.Math.Fix64 crA = MathUtils.Cross(_rA, _u);
            GGame.Math.Fix64 crB = MathUtils.Cross(_rB, _u);
            GGame.Math.Fix64 invMass = _invMassA + _invIA * crA * crA + _invMassB + _invIB * crB * crB;

            _mass = invMass != 0.0f ? 1.0f / invMass : 0.0f;

            if (Settings.EnableWarmstarting)
            {
                // Scale the impulse to support a variable time step.
                _impulse *= data.Step.dtRatio;

                Vector2 P = _impulse * _u;
                vA -= _invMassA * P;
                wA -= _invIA * MathUtils.Cross(_rA, P);
                vB += _invMassB * P;
                wB += _invIB * MathUtils.Cross(_rB, P);
            }
            else
            {
                _impulse = 0.0f;
            }

            data.Velocities[_indexA].V = vA;
            data.Velocities[_indexA].W = wA;
            data.Velocities[_indexB].V = vB;
            data.Velocities[_indexB].W = wB;
        }

        internal override void SolveVelocityConstraints(ref SolverData data)
        {
            Vector2 vA = data.Velocities[_indexA].V;
            GGame.Math.Fix64 wA = data.Velocities[_indexA].W;
            Vector2 vB = data.Velocities[_indexB].V;
            GGame.Math.Fix64 wB = data.Velocities[_indexB].W;

            // Cdot = dot(u, v + cross(w, r))
            Vector2 vpA = vA + MathUtils.Cross(wA, _rA);
            Vector2 vpB = vB + MathUtils.Cross(wB, _rB);
            GGame.Math.Fix64 C = _length - MaxLength;
            GGame.Math.Fix64 Cdot = Vector2.Dot(_u, vpB - vpA);

            // Predictive constraint.
            if (C < 0.0f)
            {
                Cdot += data.Step.inv_dt * C;
            }

            GGame.Math.Fix64 impulse = -_mass * Cdot;
            GGame.Math.Fix64 oldImpulse = _impulse;
            _impulse = Math.Min(0.0f, (float)(_impulse + impulse));
            impulse = _impulse - oldImpulse;

            Vector2 P = impulse * _u;
            vA -= _invMassA * P;
            wA -= _invIA * MathUtils.Cross(_rA, P);
            vB += _invMassB * P;
            wB += _invIB * MathUtils.Cross(_rB, P);

            data.Velocities[_indexA].V = vA;
            data.Velocities[_indexA].W = wA;
            data.Velocities[_indexB].V = vB;
            data.Velocities[_indexB].W = wB;
        }

        internal override bool SolvePositionConstraints(ref SolverData data)
        {
            Vector2 cA = data.Positions[_indexA].C;
            GGame.Math.Fix64 aA = data.Positions[_indexA].A;
            Vector2 cB = data.Positions[_indexB].C;
            GGame.Math.Fix64 aB = data.Positions[_indexB].A;

            Rot qA = new Rot(aA), qB = new Rot(aB);

            Vector2 rA = MathUtils.Mul(qA, LocalAnchorA - _localCenterA);
            Vector2 rB = MathUtils.Mul(qB, LocalAnchorB - _localCenterB);
            Vector2 u = cB + rB - cA - rA;

            GGame.Math.Fix64 length = u.Length();
            u.Normalize();
            GGame.Math.Fix64 C = length - MaxLength;

            C = MathUtils.Clamp(C, 0.0f, Settings.MaxLinearCorrection);

            GGame.Math.Fix64 impulse = -_mass * C;
            Vector2 P = impulse * u;

            cA -= _invMassA * P;
            aA -= _invIA * MathUtils.Cross(rA, P);
            cB += _invMassB * P;
            aB += _invIB * MathUtils.Cross(rB, P);

            data.Positions[_indexA].C = cA;
            data.Positions[_indexA].A = aA;
            data.Positions[_indexB].C = cB;
            data.Positions[_indexB].A = aB;

            return length - MaxLength < Settings.LinearSlop;
        }
    }
}