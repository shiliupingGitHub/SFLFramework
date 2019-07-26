
using GGame.Math;
using Microsoft.Xna.Framework;
using RoyT.AStar;
using VelcroPhysics.Collision.RayCast;

namespace GGame.Core
{
    [Interest(typeof(MoveComponent))]
    public class MoveSystem : Core.System
    {
        public override void OnUpdate()
        {

        }

        public override void OnTick()
        {
            foreach (MoveComponent moveComponent in _interestComponents)
            {

              

            }
        }

  
        

            
            



    }
}