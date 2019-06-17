using System;
using System.Xml;

namespace GGame.Core
{
    public interface MapNode : IDisposable
    {
        void Load(XmlNode node);
    }
}