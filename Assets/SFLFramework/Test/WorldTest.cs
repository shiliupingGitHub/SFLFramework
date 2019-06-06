using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using GGame;
using Vector3 = GGame.Vector3;

public class WorldTest : MonoBehaviour
{
    // Start is called before the first frame update
    private World world;
    public CinemachineVirtualCamera camera;
    public Transform startPos;
    void Start()
    {
        Enverourment.Instance.Init();
        world = new World();

        var entity = world.CreateEntity(1,1001);

        var rc = entity.GetComponent<RenderComponent>();
        camera.m_Follow = rc.GameObject.transform;
        camera.m_LookAt = rc.GameObject.transform;

        Vector3 pos;

        pos.X = (Fix64) startPos.position.x;
        pos.Y = (Fix64) startPos.position.y;
        pos.Z = (Fix64) startPos.position.z;
        rc.Pos = pos;
        rc.UpdatePostion();
        StartTick();
    }

    async void StartTick()
    {
        while (null != world)
        {
            world.Tick();
            await Task.Delay(33);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(
            (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W)  || Input.GetKeyUp(KeyCode.S))
            &&!Input.GetKey(KeyCode.A)
                &&!Input.GetKey(KeyCode.D)
            &&!Input.GetKey(KeyCode.W)
            &&!Input.GetKey(KeyCode.S)
            )
        {
            MoveCmd cmd = MoveCmd.Zero();
            
            CmdInfo info;
            
            info.Uuid = 1;
            info.Cmd = cmd;
            world.AddCatchCmd(world.FrameIndex +1, info);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            
            MoveCmd cmd = MoveCmd.Zero();
            cmd.MoveX = -1;
            CmdInfo info;
            
            info.Uuid = 1;
            info.Cmd = cmd;
            world.AddCatchCmd(world.FrameIndex +1, info);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveCmd cmd = MoveCmd.Zero();
            cmd.MoveX = 1;
            CmdInfo info;
            
            info.Uuid = 1;
            info.Cmd = cmd;
            world.AddCatchCmd(world.FrameIndex +1, info);
        }
        

        
        world.Update();
        
    }

    private void OnDestroy()
    {
        world?.Dispose();
        world = null;
    }
}
