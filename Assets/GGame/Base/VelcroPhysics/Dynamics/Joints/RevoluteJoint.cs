/*
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
using GGame.Math;
using Microsoft.Xna.Framework;
using VelcroPhysics.Dynamics.Solver;
using VelcroPhysics.Shared;
using VelcroPhysics.Utilities;

namespace VelcroPhysics.Dynamics.Joints
{
    /// <summary>
    /// A revolute joint constrains to bodies to share a common point while they
    /// are free to rotate about the point. The relative rotation about the shared
    /// point is the joint angle. You can limit the relative rotation with
    /// a joint limit that specifies a lower and upper angle. You can use a motor
    /// to drive the relative rotation about the shared point. A maximum motor torque
    /// is provided so that infinite forces are not generated.
    /// </summary>
    public class RevoluteJoint : Joint
    {
        private bool _enableLimit;

        private bool _enableMotor;

        // Solver shared
        private Vector3 _impulse;

        // Solver temp
        private int _indexA;

        private int _indexB;
        private GGame.Math.Fix64 _invIA;
        private GGame.Math.Fix64 _invIB;
        private GGame.Math.Fix64 _invMassA;
        private GGame.Math.Fix64 _invMassB;
        private LimitState _limitState;
        private Vector2 _localCenterA;
        private Vector2 _localCenterB;
        private GGame.Math.Fix64 _lowerAngle;
        private Mat33 _mass; // effective mass for point-to-point constraint.
        private GGame.Math.Fix64 _maxMotorTorque;
        private GGame.Math.Fix64 _motorImpulse;
        private GGame.Math.Fix64 _motorMass; // effective mass for motor/limit angular constraint.
        private GGame.Math.Fix64 _motorSpeed;
        private Vector2 _rA;
        private Vector2 _rB;
        private GGame.Math.Fix64 _referenceAngle;
        private GGame.Math.Fix64 _upperAngle;

        internal RevoluteJoint()
        {
            JointType = JointType.Revolute;
        }

        /// <summary>
        /// Constructor of RevoluteJoint.
        /// </summary>
        /// <param name="bodyA">The first body.</param>
        /// <param name="bodyB">The second body.</param>
        /// <param name="anchorA">The first body anchor.</param>
        /// <param name="anchorB">The second anchor.</param>
        /// <param name="useWorldCoordinates">Set to true if you are using world coordinates as anchors.</param>
        public RevoluteJoint(Body bodyA, Body bodyB, Vector2 anchorA, Vector2 anchorB, bool useWorldCoordinates = false)
            : base(bodyA, bodyB)
        {
            JointType = JointType.Revolute;

            if (useWorldCoordinates)
            {
                LocalAnchorA = BodyA.GetLocalPoint(anchorA);
                LocalAnchorB = BodyB.GetLocalPoint(anchorB);
            }
            else
            {
                LocalAnchorA = anchorA;
                LocalAnchorB = anchorB;
            }

            ReferenceAngle = BodyB.Rotation - BodyA.Rotation;

            _impulse = Vector3.Zero;
            _limitState = LimitState.Inactive;
        }

        /// <summary>
        /// Constructor of RevoluteJoint.
        /// </summary>
        /// <param name="bodyA">The first body.</param>
        /// <param name="bodyB">The second body.</param>
        /// <param name="anchor">The shared anchor.</param>
        /// <param name="useWorldCoordinates"></param>
        public RevoluteJoint(Body bodyA, Body bodyB, Vector2 anchor, bool useWorldCoordinates = false)
            : this(bodyA, bodyB, anchor, anchor, useWorldCoordinates) { }

        /// <summary>
        /// The local anchor point on BodyA
        /// </summary>
        public Vector2 LocalAnchorA { get; set; }

        /// <summary>
        /// The local anchor point on BodyB
        /// </summary>
        public Vector2 LocalAnchorB { get; set; }

        public override Vector2 WorldAnchorA
        {
            get { return BodyA.GetWorldPoint(LocalAnchorA); }
            set { LocalAnchorA = BodyA.GetLocalPoint(value); }
        }

        public override Vector2 WorldAnchorB
        {
            get { return BodyB.GetWorldPoint(LocalAnchorB); }
            set { LocalAnchorB = BodyB.GetLocalPoint(value); }
        }

        /// <summary>
        /// The referance angle computed as BodyB angle minus BodyA angle.
        /// </summary>
        public GGame.Math.Fix64 ReferenceAngle
        {
            get { return _referenceAngle; }
            set
            {
                WakeBodies();
                _referenceAngle = value;
            }
        }

        /// <summary>
        /// Get the current joint angle in radians.
        /// </summary>
        public GGame.Math.Fix64 JointAngle
        {
            get { return BodyB._sweep.A - BodyA._sweep.A - ReferenceAngle; }
        }

        /// <summary>
        /// Get the current joint angle speed in radians per second.
        /// </summary>
        public GGame.Math.Fix64 JointSpeed
        {
            get { return BodyB._angularVelocity - BodyA._angularVelocity; }
        }

        /// <summary>
        /// Is the joint limit enabled?
        /// </summary>
        /// <value><c>true</c> if [limit enabled]; otherwise, <c>false</c>.</value>
        public bool LimitEnabled
        {
            get { return _enableLimit; }
            set
            {
                if (_enableLimit == value)
                    return;

                WakeBodies();
                _enableLimit = value;
                _impulse.Z = 0.0f;
            }
        }

        /// <summary>
        /// Get the lower joint limit in radians.
        /// </summary>
        public GGame.Math.Fix64 LowerLimit
        {
            get { return _lowerAngle; }
            set
            {
                if (_lowerAngle == value)
                    return;

                WakeBodies();
                _lowerAngle = value;
                _impulse.Z = 0.0f;
            }
        }

        /// <summary>
        /// Get the upper joint limit in radians.
        /// </summary>
        public GGame.Math.Fix64 UpperLimit
        {
            get { return _upperAngle; }
            set
            {
                if (_upperAngle == value)
                    return;

                WakeBodies();
                _upperAngle = value;
                _impulse.Z = 0.0f;
            }
        }

        /// <summary>
        /// Is the joint motor enabled?
        /// </summary>
        /// <value><c>true</c> if [motor enabled]; otherwise, <c>false</c>.</value>
        public bool MotorEnabled
        {
            get { return _enableMotor; }
            set
            {
                if (value == _enableMotor)
                    return;

                WakeBodies();
                _enableMotor = value;
            }
        }

        /// <summary>
        /// Get or set the motor speed in radians per second.
        /// </summary>
        public GGame.Math.Fix64 MotorSpeed
        {
            set
            {
                if (value == _motorSpeed)
                    return;

                WakeBodies();
                _motorSpeed = value;
            }
            get { return _motorSpeed; }
        }

        /// <summary>
        /// Get or set the maximum motor torque, usually in N-m.
        /// </summary>
        public GGame.Math.Fix64 MaxMotorTorque
        {
            set
            {
                if (value == _maxMotorTorque)
                    return;

                WakeBodies();
                _maxMotorTorque = value;
            }
            get { return _maxMotorTorque; }
        }

        /// <summary>
        /// Get or set the current motor impulse, usually in N-m.
        /// </summary>
        public GGame.Math.Fix64 MotorImpulse
        {
            get { return _motorImpulse; }
            set
            {
                if (value == _motorImpulse)
                    return;

                WakeBodies();
                _motorImpulse = value;
            }
        }

        /// <summary>
        /// Set the joint limits, usually in meters.
        /// </summary>
        /// <param name="lower">The lower limit</param>
        /// <param name="upper">The upper limit</param>
        public void SetLimits(GGame.Math.Fix64 lower, GGame.Math.Fix64 upper)
        {
            if (lower == _lowerAngle && upper == _upperAngle)
                return;

            WakeBodies();
            _upperAngle = upper;
            _lowerAngle = lower;
            _impulse.Z = 0.0f;
        }

        /// <summary>
        /// Gets the motor torque in N-m.
        /// </summary>
        /// <param name="invDt">The inverse delta time</param>
        public GGame.Math.Fix64 GetMotorTorque(GGame.Math.Fix64 invDt)
        {
            return invDt * _motorImpulse;
        }

        public override Vector2 GetReactionForce(GGame.Math.Fix64 invDt)
        {
            Vector2 p = new Vector2(_impulse.X, _impulse.Y);
            return invDt * p;
        }

        public override GGame.Math.Fix64 GetReactionTorque(GGame.Math.Fix64 invDt)
        {
            return invDt * _impulse.Z;
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

            GGame.Math.Fix64 aA = data.Positions[_indexA].A;
            Vector2 vA = data.Velocities[_indexA].V;
            GGame.Math.Fix64 wA = data.Velocities[_indexA].W;

            GGame.Math.Fix64 aB = data.Positions[_indexB].A;
            Vector2 vB = data.Velocities[_indexB].V;
            GGame.Math.Fix64 wB = data.Velocities[_indexB].W;

            Rot qA = new Rot(aA), qB = new Rot(aB);

            _rA = MathUtils.Mul(qA, LocalAnchorA - _localCenterA);
            _rB = MathUtils.Mul(qB, LocalAnchorB - _localCenterB);

            // J = [-I -r1_skew I r2_skew]
            //     [ 0       -1 0       1]
            // r_skew = [-ry; rx]

            // Matlab
            // K = [ mA+r1y^2*iA+mB+r2y^2*iB,  -r1y*iA*r1x-r2y*iB*r2x,          -r1y*iA-r2y*iB]
            //     [  -r1y*iA*r1x-r2y*iB*r2x, mA+r1x^2*iA+mB+r2x^2*iB,           r1x*iA+r2x*iB]
            //     [          -r1y*iA-r2y*iB,           r1x*iA+r2x*iB,                   iA+iB]

            GGame.Math.Fix64 mA = _invMassA, mB = _invMassB;
            GGame.Math.Fix64 iA = _invIA, iB = _invIB;

            bool fixedRotation = (iA + iB == 0.0f);

            _mass.ex.X = mA + mB + _rA.Y * _rA.Y * iA + _rB.Y * _rB.Y * iB;
            _mass.ey.X = -_rA.Y * _rA.X * iA - _rB.Y * _rB.X * iB;
            _mass.ez.X = -_rA.Y * iA - _rB.Y * iB;
            _mass.ex.Y = _mass.ey.X;
            _mass.ey.Y = mA + mB + _rA.X * _rA.X * iA + _rB.X * _rB.X * iB;
            _mass.ez.Y = _rA.X * iA + _rB.X * iB;
            _mass.ex.Z = _mass.ez.X;
            _mass.ey.Z = _mass.ez.Y;
            _mass.ez.Z = iA + iB;

            _motorMass = iA + iB;
            if (_motorMass > 0.0f)
            {
                _motorMass = 1.0f / _motorMass;
            }

            if (_enableMotor == false || fixedRotation)
            {
                _motorImpulse = 0.0f;
            }

            if (_enableLimit && fixedRotation == false)
            {
                GGame.Math.Fix64 jointAngle = aB - aA - ReferenceAngle;
                if (Fix64.Abs(_upperAngle - _lowerAngle) < 2.0f * Settings.AngularSlop)
                {
                    _limitState = LimitState.Equal;
                }
                else if (jointAngle <= _lowerAngle)
                {
                    if (_limitState != LimitState.AtLower)
                    {
                        _impulse.Z = 0.0f;
                    }
                    _limitState = LimitState.AtLower;
                }
                else if (jointAngle >= _upperAngle)
                {
                    if (_limitState != LimitState.AtUpper)
                    {
                        _impulse.Z = 0.0f;
                    }
                    _limitState = LimitState.AtUpper;
                }
                else
                {
                    _limitState = LimitState.Inactive;
                    _impulse.Z = 0.0f;
                }
            }
            else
            {
                _limitState = LimitState.Inactive;
            }

            if (Settings.EnableWarmstarting)
            {
                // Scale impulses to support a variable time step.
                _impulse *= data.Step.dtRatio;
                _motorImpulse *= data.Step.dtRatio;

                Vector2 P = new Vector2(_impulse.X, _impulse.Y);

                vA -= mA * P;
                wA -= iA * (MathUtils.Cross(_rA, P) + MotorImpulse + _impulse.Z);

                vB += mB * P;
                wB += iB * (MathUtils.Cross(_rB, P) + MotorImpulse + _impulse.Z);
            }
            else
            {
                _impulse = Vector3.Zero;
                _motorImpulse = 0.0f;
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

            GGame.Math.Fix64 mA = _invMassA, mB = _invMassB;
            GGame.Math.Fix64 iA = _invIA, iB = _invIB;

            bool fixedRotation = (iA + iB == 0.0f);

            // Solve motor constraint.
            if (_enableMotor && _limitState != LimitState.Equal && fixedRotation == false)
            {
                GGame.Math.Fix64 Cdot = wB - wA - _motorSpeed;
                GGame.Math.Fix64 impulse = _motorMass * (-Cdot);
                GGame.Math.Fix64 oldImpulse = _motorImpulse;
                GGame.Math.Fix64 maxImpulse = data.Step.dt * _maxMotorTorque;
                _motorImpulse = MathUtils.Clamp(_motorImpulse + impulse, -maxImpulse, maxImpulse);
                impulse = _motorImpulse - oldImpulse;

                wA -= iA * impulse;
                wB += iB * impulse;
            }

            // Solve limit constraint.
            if (_enableLimit && _limitState != LimitState.Inactive && fixedRotation == false)
            {
                Vector2 Cdot1 = vB + MathUtils.Cross(wB, _rB) - vA - MathUtils.Cross(wA, _rA);
                GGame.Math.Fix64 Cdot2 = wB - wA;
                Vector3 Cdot = new Vector3(Cdot1.X, Cdot1.Y, Cdot2);

                Vector3 impulse = -_mass.Solve33(Cdot);

                if (_limitState == LimitState.Equal)
                {
                    _impulse += impulse;
                }
                else if (_limitState == LimitState.AtLower)
                {
                    GGame.Math.Fix64 newImpulse = _impulse.Z + impulse.Z;
                    if (newImpulse < 0.0f)
                    {
                        Vector2 rhs = -Cdot1 + _impulse.Z * new Vector2(_mass.ez.X, _mass.ez.Y);
                        Vector2 reduced = _mass.Solve22(rhs);
                        impulse.X = reduced.X;
                        impulse.Y = reduced.Y;
                        impulse.Z = -_impulse.Z;
                        _impulse.X += reduced.X;
                        _impulse.Y += reduced.Y;
                        _impulse.Z = 0.0f;
                    }
                    else
                    {
                        _impulse += impulse;
                    }
                }
                else if (_limitState == LimitState.AtUpper)
                {
                    GGame.Math.Fix64 newImpulse = _impulse.Z + impulse.Z;
                    if (newImpulse > 0.0f)
                    {
                        Vector2 rhs = -Cdot1 + _impulse.Z * new Vector2(_mass.ez.X, _mass.ez.Y);
                        Vector2 reduced = _mass.Solve22(rhs);
                        impulse.X = reduced.X;
                        impulse.Y = reduced.Y;
                        impulse.Z = -_impulse.Z;
                        _impulse.X += reduced.X;
                        _impulse.Y += reduced.Y;
                        _impulse.Z = 0.0f;
                    }
                    else
                    {
                        _impulse += impulse;
                    }
                }

                Vector2 P = new Vector2(impulse.X, impulse.Y);

                vA -= mA * P;
                wA -= iA * (MathUtils.Cross(_rA, P) + impulse.Z);

                vB += mB * P;
                wB += iB * (MathUtils.Cross(_rB, P) + impulse.Z);
            }
            else
            {
                // Solve point-to-point constraint
                Vector2 Cdot = vB + MathUtils.Cross(wB, _rB) - vA - MathUtils.Cross(wA, _rA);
                Vector2 impulse = _mass.Solve22(-Cdot);

                _impulse.X += impulse.X;
                _impulse.Y += impulse.Y;

                vA -= mA * impulse;
                wA -= iA * MathUtils.Cross(_rA, impulse);

                vB += mB * impulse;
                wB += iB * MathUtils.Cross(_rB, impulse);
            }

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

            GGame.Math.Fix64 angularError = 0.0f;
            GGame.Math.Fix64 positionError;

            bool fixedRotation = (_invIA + _invIB == 0.0f);

            // Solve angular limit constraint.
            if (_enableLimit && _limitState != LimitState.Inactive && fixedRotation == false)
            {
                GGame.Math.Fix64 angle = aB - aA - ReferenceAngle;
                GGame.Math.Fix64 limitImpulse = 0.0f;

                if (_limitState == LimitState.Equal)
                {
                    // Prevent large angular corrections
                    GGame.Math.Fix64 C = MathUtils.Clamp(angle - _lowerAngle, -Settings.MaxAngularCorrection, Settings.MaxAngularCorrection);
                    limitImpulse = -_motorMass * C;
                    angularError = Fix64.Abs(C);
                }
                else if (_limitState == LimitState.AtLower)
                {
                    GGame.Math.Fix64 C = angle - _lowerAngle;
                    angularError = -C;

                    // Prevent large angular corrections and allow some slop.
                    C = MathUtils.Clamp(C + Settings.AngularSlop, -Settings.MaxAngularCorrection, 0.0f);
                    limitImpulse = -_motorMass * C;
                }
                else if (_limitState == LimitState.AtUpper)
                {
                    GGame.Math.Fix64 C = angle - _upperAngle;
                    angularError = C;

                    // Prevent large angular corrections and allow some slop.
                    C = MathUtils.Clamp(C - Settings.AngularSlop, 0.0f, Settings.MaxAngularCorrection);
                    limitImpulse = -_motorMass * C;
                }

                aA -= _invIA * limitImpulse;
                aB += _invIB * limitImpulse;
            }

            // Solve point-to-point constraint.
            {
                qA.Set(aA);
                qB.Set(aB);
                Vector2 rA = MathUtils.Mul(qA, LocalAnchorA - _localCenterA);
                Vector2 rB = MathUtils.Mul(qB, LocalAnchorB - _localCenterB);

                Vector2 C = cB + rB - cA - rA;
                positionError = C.Length();

                GGame.Math.Fix64 mA = _invMassA, mB = _invMassB;
                GGame.Math.Fix64 iA = _invIA, iB = _invIB;

                Mat22 K = new Mat22();
                K.ex.X = mA + mB + iA * rA.Y * rA.Y + iB * rB.Y * rB.Y;
                K.ex.Y = -iA * rA.X * rA.Y - iB * rB.X * rB.Y;
                K.ey.X = K.ex.Y;
                K.ey.Y = mA + mB + iA * rA.X * rA.X + iB * rB.X * rB.X;

                Vector2 impulse = -K.Solve(C);

                cA -= mA * impulse;
                aA -= iA * MathUtils.Cross(rA, impulse);

                cB += mB * impulse;
                aB += iB * MathUtils.Cross(rB, impulse);
            }

            data.Positions[_indexA].C = cA;
            data.Positions[_indexA].A = aA;
            data.Positions[_indexB].C = cB;
            data.Positions[_indexB].A = aB;

            return positionError <= Settings.LinearSlop && angularError <= Settings.AngularSlop;
        }
    }
}