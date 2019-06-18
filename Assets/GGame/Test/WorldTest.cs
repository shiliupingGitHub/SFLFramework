
using System;
using Cinemachine;
using UnityEngine;
using GGame.Core;
using GGame.Hybird.Procedure;
using GGame.Math;
using Jitter.LinearMath;


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
        JVector dir = JVector.Zero;
        bool isMove = false;
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

            dir.Y = -Fix64.PI * 0.5;
            isMove = true;

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            changeCmd = true;

            dir.Y = Fix64.PI * 0.5;
            isMove = true;
        }

        if (changeCmd)
        {
            MoveCmd cmd;
            
            CmdInfo info;
            cmd.Dir = dir;
            cmd.isMove = isMove;
            info.Uuid = 10000001;
            info.Cmd = cmd;
            world?.AddCachCmde(world.FrameIndex +1, info);
        }
        
    }

    private void OnDestroy()
    {
      
    }
}
