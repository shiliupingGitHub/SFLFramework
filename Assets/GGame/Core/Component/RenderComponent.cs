using System;
using System.Xml;
using GGame.Math;


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
            var asset = ResourceManager.Instance.LoadPrefab(modelPath);

            _gameObject = UnityEngine.GameObject.Instantiate(asset);
            _animator = _gameObject.GetComponentInChildren<UnityEngine. Animator>();
#endif

        }


 
        
        public void UpdatePostion()
        {
#if CLIENT_LOGIC
            if(null != _gameObject)
                _gameObject.transform.position =new UnityEngine.Vector3((float)Entity.Pos.X, (float)Entity.Pos.Y, (float)Entity.Pos.Z);;
#endif
        }

        public void UpdateFace()
        {
#if CLIENT_LOGIC
            var dir = Entity.Forward;
            if(null != _animator)
                _animator.transform.forward =new UnityEngine.Vector3((float)dir.X, (float)dir.Y, (float)dir.Z);;
#endif
        }
        
            
        
    }
}

