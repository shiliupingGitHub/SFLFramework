using System;
using System.Xml;
using GGame.Math;

namespace GGame.Core
{
    public class RenderComponent : Component
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
        public override void Awake( World world, XmlNode node)
        {
            base.Awake(world, node);
            int  _modelId = Convert.ToInt32(node.Attributes?["model"].Value);
            var modelPath = $"entity_prefab_{_modelId}";
            
            _gameObject = GResourceServer.Instance.LoadPrefab(modelPath);

        }

        public override void Dispose()
        {
            base.Dispose();
            
            _gameObject?.Dispose();
        }
    }
}

