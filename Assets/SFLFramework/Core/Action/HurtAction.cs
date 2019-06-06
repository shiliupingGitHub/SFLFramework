using System;
using System.Collections.Generic;
using System.Xml;
using NotImplementedException = System.NotImplementedException;

namespace GGame
{
    public class HurtAction : IAction
    {
        private int _frameIndx = 0;
        public void Init(XmlNode data)
        {
            _frameIndx = Convert.ToInt32(data.Attributes?["frame"].Value);
        }

        public int FrameIndex
        {
            get { return _frameIndx; }
        }

        public void Dispose()
        {
            ObjectPool.Instance.Recycle(this);
        }
    }
}