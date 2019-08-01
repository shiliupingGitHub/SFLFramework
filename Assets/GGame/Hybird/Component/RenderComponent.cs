﻿using System;
using System.Xml;
using GGame.Math;

namespace GGame.Core
{
    public class RenderComponent : Component, IXmlAwake
    {

        private GGameObject _gameObject;
        private GAnimator _animator;
        
        public GGameObject GameObject
        {
            get { return _gameObject; }
        }



        public GAnimator Animator    {
            get { return _animator; }
        }
        public  void Awake( World world, XmlNode node)
        {
            int  _modelId = Convert.ToInt32(node.Attributes?["model"].Value);
            var modelPath = $"entity_prefab_{_modelId}";
            
            _gameObject = GResourceServer.Instance.LoadPrefab(modelPath);

            _gameObject.Position = new FixVector3(Entity.Pos.x, 0, Entity.Pos.y); ;

        }

        public override void Dispose()
        {
            base.Dispose();
            
            _gameObject?.Dispose();
        }
    }
}

