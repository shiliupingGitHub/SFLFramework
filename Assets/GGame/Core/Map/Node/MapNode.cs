using System.Xml;

namespace GGame.Core
{
    public interface MapNode
    {
        void Load(XmlNode node);
    }
}