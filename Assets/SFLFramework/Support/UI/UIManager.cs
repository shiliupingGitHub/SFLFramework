using System.Collections.Generic;
using NotImplementedException = System.NotImplementedException;

namespace GGame.Support
{
    [Support]
    public class UIManager : SingleTon<UIManager>
    {
        Dictionary<int, Frame> _frames = new Dictionary<int, Frame>();

        public void Show(int id)
        {
            Frame f = null;
            if (!_frames.TryGetValue(id, out  f))
            {
                f = new Frame();
            }
            
            f.OnShow();
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