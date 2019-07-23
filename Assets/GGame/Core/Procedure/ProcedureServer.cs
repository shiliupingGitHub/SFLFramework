using System;
using System.Collections.Generic;

namespace GGame.Core
{
    public class ProcedureServer : ServerAddType<ProcedureServer>
    {
        Dictionary<Type, IProcedure> _procedures = new Dictionary<Type, IProcedure>();
        
        protected override void OnAdd(Type type)
        {
            GGameEnv.Instance.AddInstanceType(type, _procedures);
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