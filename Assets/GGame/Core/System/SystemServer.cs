using System;
using System.Collections.Generic;

namespace GGame.Core
{
    public class SystemServer : SingleTon<SystemServer>, IAddType
    {
        readonly List<Type> _systemTypes = new List<Type>();
        public override void OnInit()
        {
            
        }


        public void AddType(Type t)
        {
            GGameEnv.Instance.AddAbstractType<System>(t, _systemTypes);
        }
        
        public void Create(World world)
        {
            foreach (var systemType in _systemTypes)
            {
                var system = ObjectPool.Instance.Fetch(systemType) as System;
                var attrs = systemType.GetCustomAttributes(typeof(InterestAttribute), false);
                
                world.AddSystem(system);
                foreach (var attr in attrs)
                {
                    var a = attr as InterestAttribute;
                    
                    world.AddIntrest(a.Interest, system);
                }
            }
        }
    }
}