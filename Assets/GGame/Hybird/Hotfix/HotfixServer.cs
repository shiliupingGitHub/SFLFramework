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
        }

        public void Init()
        {
            LuaEnv = new LuaEnv();
        }
        
    }

}