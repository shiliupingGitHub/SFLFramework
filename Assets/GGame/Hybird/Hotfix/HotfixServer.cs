using System;
using System.Reflection;
using GGame.Core;
using XLua;

namespace GGame.Hybird
{
    public class HotfixServer : SingleTon<HotfixServer>, IDisposable, IInit
    {
        public LuaEnv LuaEnv = null;
        public void Dispose()
        {
            
            LuaEnv?.Dispose();
            LuaEnv = null;
        }

        public void Init()
        {
            if (null == LuaEnv)
            {
                var lua_main = GResourceServer.Instance.Load<string>("lua_main") as string;
                
                LuaEnv = new LuaEnv();
                LuaEnv.DoString(lua_main);


            }

            
        }
        
    }

}