using System;
using System.Collections.Generic;
using System.Text;
using GGame.Math;

namespace Jitter.Dynamics
{

    // TODO: Check values, Documenation
    // Maybe some default materials, aka Material.Soft?
    public class Material
    {

        internal Fix64 kineticFriction = 0.3f;
        internal Fix64 staticFriction = 0.6f;
        internal Fix64 restitution = 0.0f;

        public Material() { }

        public Fix64 Restitution
        {
            get { return restitution; }
            set { restitution = value; }
        }

        public Fix64 StaticFriction
        {
            get { return staticFriction; }
            set { staticFriction = value; }
        }

        public Fix64 KineticFriction
        {
            get { return kineticFriction; }
            set { kineticFriction = value; }
        }

    }
}
