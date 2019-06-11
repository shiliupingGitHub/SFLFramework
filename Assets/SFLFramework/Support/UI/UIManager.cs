using System;
using System.Collections.Generic;
using NotImplementedException = System.NotImplementedException;

namespace GGame.Support
{
    [Support]
    public class UIManager : SingleTon<UIManager>
    {
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

        public override void OnInit()
        {
            int a = 0;
            int b = a;
        }
    }
}