using System;
using System.Xml;
using GGame.Math;


namespace GGame.Core
{
    public class HurtAction : IAction
    {
        private int _frameIndx = 0;
        
        public Fix64 Distance { get; set; }
        public void Init(XmlNode data)
        {
            _frameIndx = Convert.ToInt32(data.Attributes?["frame"].Value);
            Distance = (Fix64)Convert.ToSingle(data.Attributes?["distance"].Value);
        }

        public int FrameIndex
        {
            get { return _frameIndx; }
        }

        public void Dispose()
        {
            ObjectServer.Instance.Recycle(this);
        }
    }
}