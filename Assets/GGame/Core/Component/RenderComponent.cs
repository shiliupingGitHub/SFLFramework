using System;
using System.Xml;
using GGame.Math;
using UnityEngine;


namespace GGame.Core
{
    public class RenderComponent : Component
    {
#if CLIENT_LOGIC
        private UnityEngine.GameObject _gameObject;
        private UnityEngine.Animator _animator;
        private UnityEngine.CharacterController _collider;
      
        public UnityEngine. GameObject GameObject
        {
            get { return _gameObject; }
        }



        public UnityEngine. Animator Animator    {
            get { return _animator; }
        }
#endif
        public override void Awake( World world, XmlNode node)
        {
            base.Awake(world, node);
            int  _modelId = Convert.ToInt32(node.Attributes?["model"].Value);
#if CLIENT_LOGIC
            var modelPath = $"entity_prefab_{_modelId}";
            var asset = ResourceServer.Instance.LoadPrefab(modelPath);

            _gameObject = UnityEngine.GameObject.Instantiate(asset);
            _animator = _gameObject.GetComponentInChildren<UnityEngine. Animator>();
#endif

        }


 
        
        public void UpdatePostion()
        {
#if CLIENT_LOGIC
            if(null != _gameObject)
                _gameObject.transform.position =new UnityEngine.Vector3((float)Entity.Pos.x, (float)Entity.Pos.y, 0);;
#endif
        }

        public void UpdateFace()
        {
#if CLIENT_LOGIC
            if(null != _animator)
                _animator.transform.rotation = Quaternion.Euler(0, 90 * Entity.Face, 0);
#endif
        }
        
            
        
    }
}

