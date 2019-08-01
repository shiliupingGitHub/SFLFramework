

using XLua;

namespace GGame.Core
{
    [Interest(typeof(MoveComponent))]
#if Client_Logic
    [Hotfix]
#endif
    
    public class MoveSystem : System , ITick
    {


        public  void Tick()
        {
            foreach (MoveComponent moveComponent in _interestComponents)
            {

              

            }
        }

  
        

            
            



    }
}