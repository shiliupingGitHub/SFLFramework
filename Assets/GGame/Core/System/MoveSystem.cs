

namespace GGame.Core
{
    [Interest(typeof(MoveComponent))]
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