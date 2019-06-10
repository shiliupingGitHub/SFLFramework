using System.Collections.Generic;

namespace GGame
{
    public class UIManager : SingleTon<UIManager>
    {
        private List<Frame> _frames = new List<Frame>();
        private List<Frame> _topFrames = new List<Frame>();
    }
}