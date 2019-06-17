using System;
using System.Collections.Generic;
using System.Reflection;

namespace GGame.Core
{
    public class GGameEnv : SingleTon<GGameEnv>
    {
        readonly List<Type> _systemTypes = new List<Type>();
        readonly Dictionary<string, Type> _componentTypes = new Dictionary<string, Type>();
        readonly Dictionary<string, Type> _cmdTypes = new Dictionary<string, Type>();
        readonly Dictionary<string, Type> _jobTypes = new Dictionary<string, Type>();
        readonly Dictionary<string, Type> _actionTypes = new Dictionary<string, Type>();
        Dictionary<Type, List<ICmdHandler>> _cmdHandler = new Dictionary<Type, List<ICmdHandler>>();
        Dictionary<Type, IProcedure> _procedures = new Dictionary<Type, IProcedure>();
        Dictionary<string, Type> _mapNodeTypes = new Dictionary<string, Type>();
        public override void OnInit()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                AddAssembly(assembly);
            }
            
            AIManager.Instance.Init();
        }

        void AddAssembly(Assembly assembly)
        {
              var baseSystemType = typeof(System);
            var baseComponentType = typeof(Component);
            var baseCmdHandleType = typeof(ICmdHandler);
            var baseJobType = typeof(IJob);
            var baseActionType = typeof(IAction);
            var baseProcedureType = typeof(IProcedure);
            var types = assembly.GetTypes();
            var baseAutoInitType = typeof(IAutoInit);
            
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
                
                
                if (baseProcedureType.IsAssignableFrom(type) && !type.IsAbstract)
                {
                    var procedure = Activator.CreateInstance(type) as IProcedure;
                    _procedures[type] = procedure;
                }
                
                if (baseAutoInitType.IsAssignableFrom(type) )
                {

                    if (!type.IsAbstract)
                    {
                        var o = Activator.CreateInstance(type) as IAutoInit;
                    
                        o.Init();
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

        public Type GetMapNodeType(string typeName)
        {
            Type ret = null;

            _mapNodeTypes.TryGetValue(typeName, out ret);

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
        
        public T Enter<T, W>(W w) where T:IProcedure
        {
            var type = typeof(T);
            var ret = _procedures[type];
            ret.Enter(w);
            return (T)ret;
        }
        
        public T Enter<T, W1, W2>(W1 w, W2 w2) where T:IProcedure
        {
            var type = typeof(T);
            var ret = _procedures[type];
            
            ret.Enter(w, w2);
            return (T)ret;
        }
        public T Enter<T>() where T : IProcedure
        { 
            var type = typeof(T);
            var ret = _procedures[type];
            
            ret.Enter();
            return (T)ret;
        }

        public T Get<T>() where T: IProcedure
        {
            var type = typeof(T);
            IProcedure ret = null;
            _procedures.TryGetValue(type, out ret);
            
            return (T)ret;
        }
        
        
        
    }

}

