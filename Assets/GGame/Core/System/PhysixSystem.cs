

using Jitter.Collision;
using Jitter.Dynamics;
using Jitter.LinearMath;

namespace GGame.Core
{
    public class PhysixSystem :System
    {
        private Jitter.World _physixWorld;

        public Jitter.World PhysixWorld => _physixWorld;


        public PhysixSystem()
        {
            CollisionSystemPersistentSAP collisionSystemSap = new CollisionSystemPersistentSAP();
            _physixWorld = new Jitter.World(collisionSystemSap);
            _physixWorld.Gravity = JVector.Zero;
        }
        public override void OnUpdate()
        {
           
            

        }

        public override void OnTick()
        {
            _physixWorld?.Step(0.033f, false);
        }

       
    }
}