using System;
using System.Collections.Generic;
using System.Reflection;

namespace GGame.Core
{
    public class GGameEnv : SingleTon<GGameEnv>
    {
        readonly List<Type> _systemTypes = new List<Type>();
        readonly Dictionary<string, Type> _componentTypes = new Dictionary<string, Type>();
        readonly Dictionary<string, Type> _jobTypes = new Dictionary<string, Type>();
        readonly Dictionary<string, Type> _actionTypes = new Dictionary<string, Type>();
        Dictionary<Type, List<ICmdHandler>> _cmdHandler = new Dictionary<Type, List<ICmdHandler>>();
        Dictionary<Type, IProcedure> _procedures = new Dictionary<Type, IProcedure>();
        public override void OnInit()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                AddAssembly(assembly);
            }
            
            AIManager.Instance.Init();
        }

        void AddAbstractType<T>(Type type, List<Type> list)
        {
            var baseType = typeof(T);
            
            if (type.IsSubclassOf(baseType) && !type.IsAbstract)
            {
                list.Add(type);
            }
        }
        
        void AddAbstractType<T>(Type type, Dictionary<string, Type> dic)
        {
            var baseType = typeof(T);
            
            if (type.IsSubclassOf(baseType) && !type.IsAbstract)
            {
                dic[type.Name] = type;
            }
        }

        void AddInfeceType<T>(Type type, List<Type> list)
        {
            var baseType = typeof(T);
            
            if (baseType.IsAssignableFrom(type))
            {
                if (!type.IsAbstract)
                {

                    list.Add(type);

                }

            }
        }
        
        void AddInfeceType<T>(Type type, Dictionary<string,Type> dic)
        {
            var baseType = typeof(T);
            
            if (baseType.IsAssignableFrom(type))
            {
                if (!type.IsAbstract)
                {

                    dic[type.Name] = type;

                }

            }
        }

        void AddCmdHandler(Type type)
        {
            var baseCmdHandleType = typeof(ICmdHandler);
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
        }

        void AddInstanceType<T>(Type type, Dictionary<Type, T> dic)
        {
            var baseType = typeof(T);
            if (baseType.IsAssignableFrom(type) && !type.IsAbstract)
            {
                var instance = (T) Activator.CreateInstance(type);
                dic[type] = instance;
            }
        }

        void AuToInit(Type type)
        {
            var baseAutoInitType = typeof(IAutoInit);
            if (baseAutoInitType.IsAssignableFrom(type) )
            {

                if (!type.IsAbstract)
                {
                    var o = Activator.CreateInstance(type) as IAutoInit;
                    
                    o.Init();
                }
                    
            }
        }
        
        void AddAssembly(Assembly assembly)
        {
            
            var types = assembly.GetTypes();
           
            
            foreach (var type in types)
            {
                AddAbstractType<System>(type, _systemTypes);

                AddAbstractType<Component>(type, _componentTypes);

                AddInfeceType<IJob>(type, _jobTypes);

                AddInfeceType<IAction>(type, _actionTypes);


                AddCmdHandler(type);


                AddInstanceType<IProcedure>(type, _procedures);

                AuToInit(type);

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

