using System;
using System.Xml;
using GGame.Math;
using Jitter.LinearMath;
using UnityEngine;


namespace GGame.Core
{
    public class RenderComponent : Component
    {
        private int _modelId = 0;
        public Fix64 Speed { get; set; } = Fix64.Zero;
        public Fix64 MoveScale = (Fix64)1.0f;
        public Fix64 Acceleration = Fix64.Zero;
#if UNITY_2017_1_OR_NEWER
        private UnityEngine.GameObject _gameObject;
        private UnityEngine.Animator _animator;
        private UnityEngine.CharacterController _collider;
      
        public UnityEngine. GameObject GameObject
        {
            get { return _gameObject; }
        }

        public CharacterController Collider => _collider;

        public UnityEngine. Animator Animator    {
            get { return _animator; }
        }
#endif
        public override void Awake( World world, XmlNode node)
        {
            base.Awake(world, node);
            _modelId = Convert.ToInt32(node.Attributes?["model"].Value);
#if UNITY_2017_1_OR_NEWER
            var modelPath = $"entity_prefab_{_modelId}";
            var asset = ResourceManager.Instance.LoadPrefab(modelPath);

            _gameObject = UnityEngine.GameObject.Instantiate(asset);
            _animator = _gameObject.GetComponentInChildren<UnityEngine. Animator>();
            _collider = _gameObject.GetComponent<CharacterController>();
#endif

        }


 
        
        public void UpdatePostion()
        {
#if UNITY_2017_1_OR_NEWER
            
            _gameObject.transform.position =new UnityEngine.Vector3((float)Entity.Pos.X, (float)Entity.Pos.Y, (float)Entity.Pos.Z);;
#endif
        }

        public void UpdateFace()
        {
#if UNITY_2017_1_OR_NEWER
            var dir = Entity.Forward;
            _animator.transform.forward =new UnityEngine.Vector3((float)dir.X, (float)dir.Y, (float)dir.Z);;
#endif
        }
        
            
        
    }
}

