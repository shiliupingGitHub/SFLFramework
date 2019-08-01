using System;
using GGame.Core;
using XLua;

namespace GGame.Hybird
{
    [LuaCallCSharp]
    public  class Frame : IDisposable
    {
        public string Name { get; set; }
        private LuaTable scriptEnv;
        private Action _onInit,_onHide, _onShow, _onDisponse;
        public void OnShow(System.Object o)
        {
            if(null != _onShow)
                _onShow();
        }


        public void OnHide()
        {
            if(null != _onHide)
                _onHide();
        }

        public void OnInit()
        {
            var luaEnv = HotfixServer.Instance.LuaEnv;
            scriptEnv = luaEnv.NewTable();

            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();
            
            scriptEnv.Set("self", this);

            var lua = GResourceServer.Instance.Load<string>($"lua_{Name}") as string;
            
            luaEnv.DoString(lua, "ui", scriptEnv);
            _onInit = scriptEnv.Get<Action>("OnInit");
            _onHide = scriptEnv.Get<Action>("OnHide");
            _onShow = scriptEnv.Get<Action>("OnShow");
            _onDisponse = scriptEnv.Get<Action>("OnDisponse");
            
            if(null!= _onInit)
                _onInit();


        }


        public void Dispose()
        {
            ObjectServer.Instance.Recycle(this);
            _onDisponse();
            
            _onInit = null;
            _onShow = null;
            _onHide = null;
            _onDisponse = null;
            scriptEnv.Dispose();
            
        }
    }
}