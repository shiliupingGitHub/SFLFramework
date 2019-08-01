using GGame.Core;
using GGame.Hybird;
using UnityEngine;
using UnityEngine.UI;

namespace GGame.Hotfix
{
    [FrameID(0)]
    public class BattleTestFrame : Frame
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
            
            
            var collector = go.GetComponent<ReferenceCollector>();

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
           GameObject.Destroy(go);
        }
    }
}

