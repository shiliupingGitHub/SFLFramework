using System;
using System.Xml;
using GGame.Math;
using RoyT.AStar;

namespace GGame.Core
{
    public class Map
    {
        private Grid _grid;

        public Grid Grid => _grid;

        public void Load(XmlNode mapNode)
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
                var maxX = (int)global::System.Math.Ceiling(Convert.ToSingle(max[0]));
                
                var minY = (int)global::System.Math.Floor(Convert.ToSingle(min[1]));
                var maxY = (int)global::System.Math.Ceiling(Convert.ToSingle(max[1]));
                
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
    }
}