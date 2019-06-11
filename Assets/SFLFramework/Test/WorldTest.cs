using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using GGame;
using GGame.Support;
using UnityEngine.UI;

public class WorldTest : MonoBehaviour
{
    // Start is called before the first frame update
     World world;
     public CinemachineVirtualCamera camera;
    public Transform startPos;
    void Start()
    {
       
       
        var procedure = ProcedureManager.Instance.Enter<BattleTestProcedure, Transform, CinemachineVirtualCamera>(startPos, camera);
        world = procedure.world;
        StartTick();
        
        
    }

    async void StartTick()
    {
        if(null == world)
            return;
        while (null != world)
        {
            world.Tick();
            await Task.Delay(33);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(null == world)
            return;
        ;
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
        

        
        world?.Update();
        
    }

    private void OnDestroy()
    {
        world?.Dispose();
        world = null;
        HotfixManager.Instance.Dispose();
    }
}
