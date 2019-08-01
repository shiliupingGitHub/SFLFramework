using GGame.Core;
using GGame.Hybird;
using UnityEngine;
using UnityEngine.UI;

namespace GGame.Hotfix
{
    [FrameID(0)]
    public class BattleTestFrame : Frame
    {
        public HybirdGGameObject go;
        public void OnShow(System.Object o)
        {
            
        }

        public void OnHide()
        {
            
        }

        public void OnInit()
        {
            go =GResourceServer.Instance.LoadPrefab("frame_battle_test") as HybirdGGameObject;
            
            
            
            var collector = go.GameObject.GetComponent<ReferenceCollector>();

            Button b = collector.Get<GameObject>("btn_use_skill").GetComponent<Button>();
            
            b.onClick.AddListener(() =>
            {
                UseSkillCmd cmd;

                cmd.id = 1;
                CmdInfo info;
                info.Uuid = 10000001;
                info.Cmd = cmd;

            });
        }

        public void OnDestroy()
        {
           go?.Dispose();
        }
    }
}

