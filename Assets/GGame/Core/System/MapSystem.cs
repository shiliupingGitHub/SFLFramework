using System;
using System.Xml;
using GGame.Math;
using RoyT.AStar;

namespace GGame.Core
{
    public class MapSystem : System
    {
        private Grid _grid;
        
        void Load(XmlNode mapNode)
        {
            if(null == mapNode)
                return;

            var childNode = mapNode.FirstChild;
            while (null != childNode)
            {
                switch (childNode.Name)
                {
                    case "Blocks":
                        LoadBlocks(childNode);
                        break;
                }
                childNode = childNode.NextSibling;
            }
        }


        public float GetCellCost(int x, int y)
        {
            return _grid.GetCellCost(new Position(x, y));
        }

        void LoadBlocks(XmlNode node)
        {
            int mapX = Convert.ToInt32(node.Attributes["x"].Value);
            int mapY = Convert.ToInt32(node.Attributes["y"].Value);
            
            _grid = new Grid(mapX, mapY);

            var childNode = node.FirstChild;

            while (null != childNode)
            {
                var min = childNode.Attributes["min"].Value.Split(',');
                var max = childNode.Attributes["max"].Value.Split(',');

                var minX = (int)global::System.Math.Floor(Convert.ToSingle(min[0]));
                minX = global::System.Math.Max(0, minX);
                var maxX = (int)global::System.Math.Ceiling(Convert.ToSingle(max[0]));
                maxX = global::System.Math.Min(maxX, _grid.DimX -1);
                
                var minY = (int)global::System.Math.Floor(Convert.ToSingle(min[1]));
                minY = global::System.Math.Max(0, minY);
                var maxY = (int)global::System.Math.Ceiling(Convert.ToSingle(max[1]));
                maxY = global::System.Math.Min(maxY, _grid.DimY - 1);
                for(int x = minX + 1; x < maxX; x++)
                {
                    for (int y = minY; y < maxY; y++)
                    {
                        _grid.BlockCell(new Position(x, y));
                    }
                    
                }
                
                
                childNode = childNode.NextSibling;
            }
        }
        
        public void LoadMap(int configId)
        {
            var configPath = $"map_config_{configId}";
            var configText = ResourceManager.Instance.LoadText(configPath);
            
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(configText);
            
            Load(doc.FirstChild);
            
        }
        public override void OnUpdate()
        {
            
        }

        public override void OnTick()
        {
            
        }
    }
}