using System;
using System.Xml;
using UnityEngine;

namespace GGame
{
    public class RenderComponent : Component
    {
        private int _modelId = 0;
        private Vector3 _pos = Vector3.Zero;
        private Vector3 _forward = new Vector3(Fix64.One, Fix64.Zero, Fix64.Zero);
        
#if !SERVER
        private UnityEngine.GameObject _gameObject;
        private UnityEngine.Animator _animator;

        public GameObject GameObject
        {
            get { return _gameObject; }
        }

        public Animator Animator    {
            get { return _animator; }
        }
#endif
        public override void Awake( World world, XmlNode node)
        {
            base.Awake(world, node);
            _modelId = Convert.ToInt32(node.Attributes?["model"].Value);
#if !SERVER
            var modelPath = $"EntityPrefab/entity_prefab_{_modelId}";
            var asset = ResourceManager.Instance.LoadEntityPrefab(modelPath);

            _gameObject = UnityEngine.GameObject.Instantiate(asset);
            _animator = _gameObject.GetComponentInChildren<Animator>();

            UpdatePostion();
#endif

        }


        public Vector3 Pos
        {
            set
            {
                _pos = value;
            }
            get { return _pos; }
            
        }


        public void UpdatePostion()
        {
#if !SERVER
            
            _gameObject.transform.position =new UnityEngine.Vector3((float)_pos.X, (float)_pos.Y, (float)_pos.Z);;
#endif
        }
        
            
        
    }
}

