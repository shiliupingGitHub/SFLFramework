using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using GGame;
using GGame.Core;
using GGame.Hybird.Procedure;
using GGame.Support;

public class WorldTest : MonoBehaviour
{
    // Start is called before the first frame update
    public CinemachineVirtualCamera camera;
    public Transform startPos;
    void Start()
    {
        ResourceManager.Instance.Init();
        WorldEnv.Instance.Enter<BattleTestProcedure, Transform, CinemachineVirtualCamera>(startPos, camera);

    }

 
    // Update is called once per frame
    void Update()
    {

        var world = WorldEnv.Instance.Get<BattleTestProcedure>().world;
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
