using System;
using System.Collections.Generic;
using System.Reflection;

namespace GGame.Core
{
    public class GGameEnv : SingleTon<GGameEnv>, IInit
    {
        

        public void AddAbstractType<T>(Type type, List<Type> list)
        {
            var baseType = typeof(T);
            
            if (type.IsSubclassOf(baseType) && !type.IsAbstract)
            {
                list.Add(type);
            }
        }
        
       public void AddAbstractType<T>(Type type, Dictionary<string, Type> dic)
        {
            var baseType = typeof(T);
            
            if (type.IsSubclassOf(baseType) && !type.IsAbstract)
            {
                dic[type.Name] = type;
            }
        }

        public void AddInfeceType<T>(Type type, List<Type> list)
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
        
        public void AddInfeceType<T>(Type type, Dictionary<string,Type> dic)
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


        public void AddInstanceType<T>(Type type, Dictionary<Type, T> dic)
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
                SystemServer.Instance.AddType(type);
                ComponentServer.Instance.AddType(type);
                JobServer.Instance.AddType(type);
                ActionServer.Instance.AddType(type);
                CmdServer.Instance.AddType(type);
                AuToInit(type);

            }
        }


        public void Init()
        {
            AddAssembly(typeof(GGameEnv).Assembly);
            
            AIServer.Instance.Init();
        }
    }

}

