using System;
using System.Collections.Generic;

namespace GGame.Core
{
    public class CmdServer : SingleTon<CmdServer>,IAddType
    {
        Dictionary<Type, List<ICmdHandler>> _cmdHandler = new Dictionary<Type, List<ICmdHandler>>();
        public override void OnInit()
        {
            
        }

        public void AddType(Type t)
        {
            AddCmdHandler(t);
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
    }
}