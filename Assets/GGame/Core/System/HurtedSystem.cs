using System.Collections.Generic;
using GGame.Math;

namespace GGame.Core
{
    public struct Hurt
    {
        public Entity _Entity;
        public HurtAction _HurtAction;

    };
    [Interest(typeof(HurtedComponent))]
    public class HurtedSystem : System, ITick
    {
        private List<Hurt> _list = new List<Hurt>();
        public void AddHurt( Hurt hurt)
        {
            _list.Add(hurt);
        }
        


        public  void Tick()
        {
            foreach (var hurt in _list)
            {
                foreach (HurtedComponent hc in _interestComponents)
                {

                    

                  
                    
                }
            }
            _list.Clear();
        }
    }
}