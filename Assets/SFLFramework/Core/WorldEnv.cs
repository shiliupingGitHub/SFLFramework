using System;
using System.Collections.Generic;
using System.Reflection;

namespace GGame
{
    public class WorldEnv : SingleTon<WorldEnv>
    {
        readonly List<Type> _systemTypes = new List<Type>();
        readonly Dictionary<string, Type> _componentTypes = new Dictionary<string, Type>();
        readonly Dictionary<string, Type> _cmdTypes = new Dictionary<string, Type>();
        readonly Dictionary<string, Type> _jobTypes = new Dictionary<string, Type>();
        readonly Dictionary<string, Type> _actionTypes = new Dictionary<string, Type>();
        Dictionary<Type, List<ICmdHandler>> _cmdHandler = new Dictionary<Type, List<ICmdHandler>>();
        public override void OnInit()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                AddAssembly(assembly);
            }
        }

        void AddAssembly(Assembly assembly)
        {
              var baseSystemType = typeof(System);
            var baseComponentType = typeof(Component);
            var baseCmdHandleType = typeof(ICmdHandler);
            var baseJobType = typeof(IJob);
            var baseActionType = typeof(IAction);
            var types = assembly.GetTypes();

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

                        List<ICmdHandler> cache = null;

                        if (!_cmdHandler.TryGetValue(handler.Type, out cache))
                        {
                            cache = new List<ICmdHandler>();
                            _cmdHandler[handler.Type] = cache;
                        }
                        cache.Add(handler);
                        
                    }

                }
                
                if (baseJobType.IsAssignableFrom(type))
                {
                    if (!type.IsAbstract)
                    {

                        _jobTypes[type.Name] = type;

                    }

                }
                
                if (baseActionType.IsAssignableFrom(type))
                {
                    if (!type.IsAbstract)
                    {

                        _actionTypes[type.Name] = type;

                    }

                }
                
            }
        }

        public void ExecuteCmd<T>(T a) 
        {
            var type = a.GetType();

            if (_cmdHandler.TryGetValue(type, out var cache))
            {
                foreach (var handler in cache)
                {
                    handler.Execute(a);
                }
            }
        }
        
        public void ExecuteCmd<T,K,W>(T a, K b, W o)
        {
            var type = o.GetType();

            if (_cmdHandler.TryGetValue(type, out var cache))
            {
                foreach (var handler in cache)
                {
                    handler.Execute(a, b, o);
                }
            }
        }
        
        public Type GetComponentType(string typeName)
        {
            Type ret = null;

            _componentTypes.TryGetValue(typeName, out ret);
            return ret;
        }

        public Type GetJobType(string typeName)
        {
            Type ret = null;

            _jobTypes.TryGetValue(typeName, out ret);
            
            return ret;
        }
        
        public Type GetActionType(string typeName)
        {
            Type ret = null;

            _actionTypes.TryGetValue(typeName, out ret);
            
            return ret;
        }

        public void CreateWorldSystem(World world)
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

