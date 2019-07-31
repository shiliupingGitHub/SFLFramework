
using System;
using Cinemachine;
using UnityEngine;
using GGame.Core;
using GGame.Hybird.Procedure;
using GGame.Math;

public class WorldTest : MonoBehaviour
{
    private World _World;
    // Start is called before the first frame update
    public CinemachineVirtualCamera camera;
    public Transform startPos;
    void Start()
    {
       
        _World = new World(true);
        
        _World.GetSystem<MapSystem>().LoadMap(1001);
    }

 
    // Update is called once per frame
    void Update()
    {
      

        
        var world =_World;
        bool isLeft = true;
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

            isLeft = true;
            isMove = true;

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            changeCmd = true;

            isLeft = false;
            isMove = true;
        }

        if (changeCmd)
        {
            MoveCmd cmd;
            
            CmdInfo info;
            cmd.isLeft = isLeft;
            cmd.isMove = isMove;
            info.Uuid = 10000001;
            info.Cmd = cmd;
            world?.AddCachCmde(world.FrameIndex +1, info);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            JumpCmd cmd;
            
            CmdInfo info;
            info.Uuid = 10000001;
            info.Cmd = cmd;
            world?.AddCachCmde(world.FrameIndex +1, info);
        }
        
        
    }

    private void OnDestroy()
    {
        _World?.Dispose();
    }
}
