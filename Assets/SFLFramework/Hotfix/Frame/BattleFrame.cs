using GGame.Support;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace GGame.Hotfix
{
    [FrameID(1)]
    public class BattleFrame : Frame
    {
        public GameObject go;
        public void OnShow()
        {
            
        }

        public void OnHide()
        {
            
        }

        public void OnInit()
        {
            var asset = ResourceManager.Instance.LoadPrefab("Frame/battle_frame");

            go = GameObject.Instantiate(asset);
        }

        public void OnDestroy()
        {
           GameObject.Destroy(go);
        }
    }
}

