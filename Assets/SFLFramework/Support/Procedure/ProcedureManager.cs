using System;
using System.Collections.Generic;
using NotImplementedException = System.NotImplementedException;

namespace GGame.Support
{
    [Support]
    public class ProcedureManager : SingleTon<ProcedureManager>
    {
        Dictionary<Type, IProcedure> _procedures = new Dictionary<Type, IProcedure>();
        public override void OnInit()
        {
            var assembly = typeof(IProcedure).Assembly;
            var types = assembly.GetTypes();
            var baseProcedureType = typeof(IProcedure);

            foreach (var type in types)
            {
                if (baseProcedureType.IsAssignableFrom(type) && !type.IsAbstract)
                {
                    var procedure = Activator.CreateInstance(type) as IProcedure;
                    _procedures[type] = procedure;
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

        public T Get<T>()
        {
            var type = typeof(T);
            var ret = _procedures[type];

            return (T)ret;
        }
        
    }
}