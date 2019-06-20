

using System;
using GGame.Math;

namespace GGame.Core
{
    [Interest(typeof(RenderComponent))]
    public class RenderSystem : Core.System
    {
        public override void OnUpdate()
        {
            
        
        }

        public override void OnTick()
        {
#if CLIENT_LOGIC
            foreach (RenderComponent renderComponent in _interestComponents)
            {
                renderComponent.UpdatePostion();
                renderComponent.UpdateFace();

                renderComponent.Animator.SetFloat("SpeedX", (float)renderComponent.Entity.MoveSpeedX);

            }
#endif
        }
    }
}


