using System;
using System.Collections.Generic;
using GGame.Core;
using GGame.Hybird;


namespace GGame.Hybird
{
    public class UISever : SingleTon<UISever>
    {
        public UISever()
        {
            _instance = this;
        }
        Dictionary<int, Frame> _frames = new Dictionary<int, Frame>();

        public Func<int, Frame> OnNewFrame;
        public void Show(int id)
        {
            Frame f = null;
            if (!_frames.TryGetValue(id, out  f))
            {
                if (null != OnNewFrame)
                {
                    f = OnNewFrame(id);

                    if (null != f)
                    {
                        _frames[id] = f;
                        f.OnInit();
                    }
                }
            }
            
            f?.OnShow();
        }

        public void Hide(int id)
        {
            if (_frames.TryGetValue(id, out var frame))
            {
                frame.OnHide();
            }
        }

        
    }
}