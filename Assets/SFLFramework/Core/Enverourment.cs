using System;
using System.Collections.Generic;
using System.Reflection;

namespace GGame
{
    public class Enverourment : SingleTon<Enverourment>
    {
        readonly List<Type> _systemTypes = new List<Type>();
        readonly Dictionary<string, Type> _componentTypes = new Dictionary<string, Type>();
        public void Init()
        {
            var baseSystemType = typeof(System);
            var baseComponentType = typeof(Component);
            var types = baseSystemType.Assembly.GetTypes();

            foreach (var type in types)
            {
                if (type.IsSubclassOf(baseSystemType) && type != baseSystemType)
                {
                    _systemTypes.Add(type);
                }
                
                if (type.IsSubclassOf(baseComponentType) && type != baseComponentType)
                {
                    _componentTypes[type.Name] = type;
                }
            }
        }

        public Type GetComponentType(string typeName)
        {
            Type ret = null;

            _componentTypes.TryGetValue(typeName, out ret);
            return ret;
        }
        

        public void CreateWorldSystem(World world)
        {
            foreach (var systemType in _systemTypes)
            {
                var system = Activator.CreateInstance(systemType) as System;
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

