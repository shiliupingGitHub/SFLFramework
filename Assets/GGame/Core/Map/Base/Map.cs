using System;
using System.Collections.Generic;
using System.Xml;

namespace GGame.Core
{
    public class Map : IDisposable
    {
        List<MapNode> _bodies = new List<MapNode>();
        public void Dispose()
        {
            foreach (var body in _bodies)
            {
                body.Dispose();
            }
            
            _bodies.Clear();
            ObjectPool.Instance.Recycle(this);
        }

        
        public void Load(string config)
        {
            XmlDocument document = new XmlDocument();
            
            document.LoadXml(config);

            var map = document.FirstChild;
            var node = map.FirstChild;

            while (null != node)
            {
                var nodeType = GGameEnv.Instance.GetMapNodeType(node.Name);

                if (null != nodeType)
                {
                    MapNode nodeIns = ObjectPool.Instance.Fetch(nodeType) as MapNode;
                    
                    nodeIns.Load(node);
                }
                
                node = node.NextSibling;
            }
        }
    }
    
    
}