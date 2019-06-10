using System;
using System.Xml;
using UnityEngine;

namespace GGame
{
    public class RenderComponent : Component
    {
        private int _modelId = 0;
        private FixVector3 _pos = FixVector3.Zero;

        public FixVector3 MoveDir
        {
            get;
            set;
        } = FixVector3.Zero;
        
        public FixVector3 Face { get; set; } = new FixVector3((Fix64)1, (Fix64)0,(Fix64)0);
        public Fix64 Speed { get; set; } = Fix64.Zero;
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
            
#endif

        }


        public FixVector3 Pos
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
            
            _gameObject.transform.position =new UnityEngine.Vector3((float)_pos.x, (float)_pos.y, (float)_pos.z);;
#endif
        }

        public void UpdateFace()
        {
#if !SERVER
            _animator.transform.forward =new UnityEngine.Vector3((float)Face.x, (float)Face.y, (float)Face.z);;
#endif
        }
        
            
        
    }
}

