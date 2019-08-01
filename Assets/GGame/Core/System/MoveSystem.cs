

using XLua;

namespace GGame.Core
{
    [Interest(typeof(MoveComponent))]

    [Hotfix]

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