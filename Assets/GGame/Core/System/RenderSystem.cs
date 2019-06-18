

using System;
using GGame.Math;

namespace GGame.Core
{
    [Interest(typeof(RenderComponent))]
    public class RenderSystem : Core.System
    {
        public override void OnUpdate()
        {
            foreach (RenderComponent component in _interestComponents)
            {
                component.UpdateFace();
                component.UpdatePostion();
                
            }
        
        }

        public override void OnTick()
        {
           
        }
    }
}


