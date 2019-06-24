using System;
using System.Xml;
using GGame.Math;
using Microsoft.Xna.Framework;
using RoyT.AStar;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Shared;

namespace GGame.Core
{
    public class MapSystem : System
    {

        public VelcroPhysics.Dynamics.World _physixWorld  = new VelcroPhysics.Dynamics.World(Vector2.Zero);
        
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

        public override void Dispose()
        {
            base.Dispose();
        }

        void LoadBlocks(XmlNode node)
        {

            
            var childNode = node.FirstChild;

            while (null != childNode)
            {

                float width = Convert.ToSingle(childNode.Attributes["width"].Value);
                float height = Convert.ToSingle(childNode.Attributes["height"].Value);
                Vector2 postion = Vector2.Zero;

                var strPos = childNode.Attributes["postion"].Value.Split(',');

                postion.X = Convert.ToSingle(strPos[0]);
                postion.Y = Convert.ToSingle(strPos[1]);
                
                float rotation = 0;
                float density = 1.0f;

                BodyFactory.CreateRectangle(_physixWorld, width, height, density, postion, rotation, BodyType.Static);
                
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