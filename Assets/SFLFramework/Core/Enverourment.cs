using System;
using System.Collections.Generic;
using System.Reflection;

namespace GGame
{
    public class Enverourment : SingleTon<Enverourment>
    {
        readonly List<Type> _systemTypes = new List<Type>();
        readonly Dictionary<string, Type> _componentTypes = new Dictionary<string, Type>();
        readonly Dictionary<string, Type> _cmdTypes = new Dictionary<string, Type>();
        Dictionary<Type, ICmdHandler> _cmdHandler = new Dictionary<Type, ICmdHandler>();
        public void Init()
        {
            var baseSystemType = typeof(System);
            var baseComponentType = typeof(Component);
            var baseCmdHandleType = typeof(ICmdHandler);
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

                var attrs = type.GetCustomAttributes(typeof(CmdAttribute), false);

                foreach (var attr in attrs)
                {
                    var a = attr as CmdAttribute;
                    _cmdTypes[a.Op] = type;
                }

                if (baseCmdHandleType.IsAssignableFrom(type))
                {
                    if (!type.IsAbstract)
                    {
                                            
                        var handler = Activator.CreateInstance(type) as ICmdHandler;

                        _cmdHandler[handler.Type] = handler;
                    }

                }
            }
        }

        public ICmdHandler GetCmdHandler(Type t)
        {
            ICmdHandler handler;

            _cmdHandler.TryGetValue(t, out handler);

            return handler;
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

