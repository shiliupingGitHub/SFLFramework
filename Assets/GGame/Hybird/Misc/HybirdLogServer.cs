using GGame.Core;

namespace GGame.Hybird
{
    public class HybirdLogServer : LogServer, IAutoInit
    {
        public void Init()
        {
            
        }
        
        
                
        public override void Debug(string info)
        {

            UnityEngine.Debug.Log(info);

        }

        public override void Error(string info)
        {

            UnityEngine.Debug.LogError(info);

        }
    }
}