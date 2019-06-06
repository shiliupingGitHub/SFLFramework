using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using GGame;
using UnityEngine.UI;
using Vector3 = GGame.Vector3;

public class WorldTest : MonoBehaviour
{
    // Start is called before the first frame update
    private World world;
    public CinemachineVirtualCamera camera;
    public Transform startPos;
    public Button btn_use_skill;
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
        btn_use_skill.onClick.AddListener(() =>
        {
            CmdInfo cmdInfo;
            UseSkillCmd cmd;

            cmd.id = 1;
            cmdInfo.Uuid = 1;
            cmdInfo.Cmd = cmd;
            
            world?.AddCachCmde(world.FrameIndex +1, cmdInfo);
        });
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
        int moveX = 0;
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
            moveX = -1;

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            changeCmd = true;
            moveX = 1;
        }

        if (changeCmd)
        {
            MoveCmd cmd = MoveCmd.Zero();
            
            CmdInfo info;
            cmd.MoveX = moveX;
            
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
    }
}
