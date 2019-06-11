using GGame.Support;
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
            var asset = ResourceManager.Instance.LoadPrefab("Frame/battle_frame");

            go = GameObject.Instantiate(asset);
            var collector = go.GetComponent<ReferenceCollector>();

            Button b = collector.Get<GameObject>("btn_use_skill").GetComponent<Button>();
            
            b.onClick.AddListener(() =>
            {
                UseSkillCmd cmd;

                cmd.id = 1;
                CmdInfo info;
                info.Uuid = 1;
                info.Cmd = cmd;

                var procedure = ProcedureManager.Instance.Get<BattleTestProcedure>();
                
                procedure.world.AddCachCmde(procedure.world.FrameIndex+1, info);
            });
        }

        public void OnDestroy()
        {
           GameObject.Destroy(go);
        }
    }
}

