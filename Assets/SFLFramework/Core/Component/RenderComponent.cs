using System;
using System.Xml;
using UnityEngine;

namespace GGame
{
    public class RenderComponent : Component
    {
        private int _modelId = 0;
        private Vector3 _pos = Vector3.Zero;
        private Vector3 _rotaion = Vector3.Zero;
        
#if !SERVER
        private UnityEngine.GameObject _gameObject;

        public GameObject GameObject
        {
            get { return _gameObject; }
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

            UpdatePostion();
#endif

        }


        public Vector3 Pos
        {
            set
            {
                _pos = value;
                UpdatePostion();
            }
            get { return _pos; }
            
        }

        public Vector3 Rotaion
        {
            get => _rotaion;
            set { _rotaion = value;
                UpdateRotaion();
            }
        }

        private void UpdateRotaion()
        {
#if !SERVER
            _gameObject.transform.rotation =UnityEngine.Quaternion.Euler((float)_rotaion.X, (float)_rotaion.Y, (float)_rotaion.Z );
#endif
        }

        private void UpdatePostion()
        {
#if !SERVER
            var tr_pos = _gameObject.transform.position;

            tr_pos.x = (float)_pos.X;
            tr_pos.y = (float)_pos.Y;
            tr_pos.z = (float)_pos.Z;

            _gameObject.transform.position = tr_pos;
#endif
        }
        
            
        
    }
}

