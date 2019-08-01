using System;
using System.Xml;
using GGame.Math;

namespace GGame.Core
{
    public class RenderComponent : Component, IXmlAwake
    {
        

        public  void Awake( World world, XmlNode node)
        {
            int  _modelId = Convert.ToInt32(node.Attributes?["model"].Value);
            var modelPath = $"entity_prefab_{_modelId}";
            
        }
        
    }
}

