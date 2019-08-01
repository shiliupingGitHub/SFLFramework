
using System;
using System.Collections.Generic;
using System.Xml;


namespace GGame.Core
{
    public class MapSystem : System
    {

        void Load(XmlNode mapNode)
        {
            if(null == mapNode)
                return;
            
            var childNode = mapNode.FirstChild;
            while (null != childNode)
            {
                switch (childNode.Name)
                {
                    case "NavMesh":
                       
                        break;
                }
                childNode = childNode.NextSibling;
            }
        }
        
        public void LoadMap(int configId)
        {
            var configPath = $"map_config_{configId}";
            var configText = GResourceServer.Instance.LoadText(configPath);
            
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(configText);
            
            Load(doc.FirstChild);
            
        }
    }
}