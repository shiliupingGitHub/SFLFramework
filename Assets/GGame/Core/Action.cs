using System;
using System.Xml;

namespace GGame.Core
{
    public interface IAction : IDisposable
    {

        void Init(XmlNode data);
        int FrameIndex { get; }
    }
    
}