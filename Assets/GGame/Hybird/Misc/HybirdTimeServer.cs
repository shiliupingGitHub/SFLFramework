using GGame.Core;
using GGame.Math;

namespace GGame.Hybird
{
    public class HybirdTimeServer : TimeServer
    {
        public override Fix64 DeltaTime
        {
            get { return  (Fix64)UnityEngine.Time.deltaTime; }
            
        }
    }
}