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

        VelcroPhysics.Dynamics.World _physixWorld  = new VelcroPhysics.Dynamics.World(Vector2.Zero);
        
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

        public bool GetHitPoint(FixVector2 cur, FixVector2 target, out FixVector2 point)
        {
            var ret =  World.GetSystem<MapSystem>()._physixWorld
                .RayCast(new Vector2((float)cur.x, (float)cur.y), new Vector2((float)(target.x), (float)target.y));

            if (ret.Count > 0)
            {
                var fraction = ret[0].Item4;
                point = new FixVector2(ret[0].Item2.X, ret[0].Item2.Y);

                for (int i = 1; i < ret.Count; i++)
                {
                    if (ret[i].Item4 < fraction)
                    {
                        fraction = ret[i].Item4;
                        point = new FixVector2(ret[i].Item2.X, ret[i].Item2.Y);
                    }
                    
                }
                
                
                return true;
            }
            else
            {
               point = FixVector2.Zero;
                
                return false;
            }
         
        }

        void LoadBlocks(XmlNode node)
        {

            
            var childNode = node.FirstChild;

            while (null != childNode)
            {
                switch ( childNode.Name)
                {
                    case "Box":
                        LoadBox(childNode);
                        break;
                }
            
                
              
                childNode = childNode.NextSibling;
            }
        }

        void LoadBox(XmlNode node)
        {
            float width = Convert.ToSingle(node.Attributes["width"].Value);
            float height = Convert.ToSingle(node.Attributes["height"].Value);
            Vector2 postion = Vector2.Zero;

            var strPos = node.Attributes["postion"].Value.Split(',');

            postion.X = Convert.ToSingle(strPos[0]);
            postion.Y = Convert.ToSingle(strPos[1]);
                
            float rotation = 0;
            float density = 1.0f;

            BodyFactory.CreateRectangle(_physixWorld, width, height, density, postion, rotation, BodyType.Static);
        }
        
        public void LoadMap(int configId)
        {
            var configPath = $"map_config_{configId}";
            var configText = ResourceServer.Instance.LoadText(configPath);
            
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