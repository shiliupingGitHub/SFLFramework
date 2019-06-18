

using Jitter.Collision;
using Jitter.Dynamics;

namespace GGame.Core
{
    public class PhysixSystem :System
    {
        private Jitter.World _physixWorld;

        public Jitter.World PhysixWorld => _physixWorld;


        public PhysixSystem()
        {
            CollisionSystemSAP collisionSystemSap = new CollisionSystemSAP();
            _physixWorld = new Jitter.World(collisionSystemSap);
        }
        public override void OnUpdate()
        {
            RigidBody bd;
            

        }

        public override void OnTick()
        {
            _physixWorld?.Step(0.033f, false);
        }

       
    }
}