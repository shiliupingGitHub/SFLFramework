using System.Xml;

namespace GGame.Core
{
    public interface IXmlAwake
    {
        void Awake(World world, XmlNode node);
    }
}