using System;
using System.Xml;
using GGame.Math;
using UnityEngine;

namespace GGame.Core
{
    public class RenderComponent : Component, IXmlAwake
    {
        public GameObject  GameObject { get; set; }
        public  void Awake( World world, XmlNode node)
        {
            int  _modelId = Convert.ToInt32(node.Attributes?["model"].Value);
            var modelPath = $"entity_prefab_{_modelId}";
            
            GameObject = GResourceServer.Instance.Load<GameObject>(modelPath) as GameObject;

        }

        public override void Dispose()
        {
            base.Dispose();
            if(null != GameObject)
                GameObject.Destroy(GameObject);
        }
    }
}

