using System;
using System.Collections.Generic;
using behaviac;

namespace GGame.Core
{
    public class ObjectServer : SingleTon<ObjectServer>
    {
        
        Dictionary<Type, List<Object>> _cache = new Dictionary<Type, List<object>>();
        public T Fetch<T>() where T: IDisposable , new()
        {

            return (T)Fetch(typeof(T));
        }

        public Object Fetch(Type type)
        {
            if (_cache.TryGetValue(type, out var cache))
            {

                if (cache.Count > 0)
                {
                    Object o = cache[0];
                    cache.RemoveAt(0);
                    return o;
                }
                
            }
            
            var ret = Activator.CreateInstance(type);
            return ret;
        }

        public void Recycle(Object o)
        {

            List<Object> cache = null;
            Type type = o.GetType();

            if (!_cache.TryGetValue(type, out cache))
            {
                cache = new List<object>();

                _cache[type] = cache;
            }
            
            cache.Add(o);
        }

        
    }
}