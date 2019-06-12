

namespace GGame.Core
{
    [Interest(typeof(FreeAIComponent))]
    public class FreeAISystem : System
    {
        public override void OnUpdate()
        {
            
        }

        public override void OnTick()
        {
            foreach (FreeAIComponent component in _interestComponents)
            {

                if (null != component.Agent)
                {
                    behaviac.EBTStatus status = behaviac.EBTStatus.BT_RUNNING;
                    
                    while (status == behaviac.EBTStatus.BT_RUNNING)
                    {
                        
                        status = component.Agent.btexec();
                    }
                }
                
            }
        }
    }
}