
using System;
using Cinemachine;
using UnityEngine;
using GGame.Core;
using GGame.Hybird.Procedure;


public class WorldTest : MonoBehaviour
{
    // Start is called before the first frame update
    public CinemachineVirtualCamera camera;
    public Transform startPos;
    void Start()
    {
        ResourceManager.Instance.Init();
        GGameEnv.Instance.Enter<BattleTestProcedure, Transform, CinemachineVirtualCamera>(startPos, camera);
        var d = skill_config.Dic[1];
     
        SoundManager.Instance.DoEvent("event:/attack_01");
    }

 
    // Update is called once per frame
    void Update()
    {
      
        var procedure = GGameEnv.Instance.Get<BattleTestProcedure>();
        if(null == procedure)
            return;
        
        var world =procedure.world;
        FixVector3 dir = FixVector3.Zero;
        bool changeCmd = (Input.GetKeyUp(KeyCode.A) ||
                          Input.GetKeyUp(KeyCode.D) || 
                          Input.GetKeyUp(KeyCode.W)  ||
                          Input.GetKeyUp(KeyCode.S)
                         )
                         &&!Input.GetKey(KeyCode.A)
                         &&!Input.GetKey(KeyCode.D)
                         &&!Input.GetKey(KeyCode.W)
                         &&!Input.GetKey(KeyCode.S);

        if (Input.GetKeyDown(KeyCode.A))
        {
            changeCmd = true;

            dir.x = -Fix64.One;

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            changeCmd = true;

            dir.x = Fix64.One;
        }

        if (changeCmd)
        {
            MoveCmd cmd;
            
            CmdInfo info;
            cmd.Dir = dir;
            
            info.Uuid = 1;
            info.Cmd = cmd;
            world?.AddCachCmde(world.FrameIndex +1, info);
        }
        
    }

    private void OnDestroy()
    {
      
    }
}
