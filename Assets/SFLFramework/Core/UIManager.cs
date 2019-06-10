using System.Collections.Generic;

namespace GGame
{
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
        
    }
}