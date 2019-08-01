using System;
using System.Collections.Generic;
using GGame.Core;
using GGame.Hybird;
using XLua;


namespace GGame.Hybird
{
    [LuaCallCSharp]
    public class UISever : SingleTon<UISever>
    {
        Dictionary<string, Frame> _frames = new Dictionary<string, Frame>();

        public Func<int, Frame> OnNewFrame;
        public void Show(string name, System.Object o)
        {
            Frame f = null;
            if (!_frames.TryGetValue(name, out  f))
            {
                f = ObjectServer.Instance.Fetch<Frame>();
                f.Name = name;
                f.OnInit();
            }
            
            f?.OnShow(o);
        }

        public void Hide(string name)
        {
            if (_frames.TryGetValue(name, out var frame))
            {
                frame.OnHide();
            }
        }

        
    }
}