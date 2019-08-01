

using System;
using GGame.Math;
using UnityEngine.AI;

namespace GGame.Core
{
    [Interest(typeof(RenderComponent))]
    public class RenderSystem : Core.System , IUpdate
    {
        public void Update()
        {
            foreach (RenderComponent rc in _interestComponents)
            {
                
               
            }
        }
    }
}


