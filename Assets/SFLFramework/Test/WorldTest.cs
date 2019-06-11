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
    private World world;
    private Entity entity;
    public CinemachineVirtualCamera camera;
    public Transform startPos;
    public Button btn_use_skill;
    void Start()
    {
        HotfixManager.Instance.Init();
        world = new World();

        entity = world.CreateEntity(1,1001);
        entity.Camp = 0;
        
        var rc = entity.GetComponent<RenderComponent>();
        camera.m_Follow = rc.GameObject.transform;
        camera.m_LookAt = rc.GameObject.transform;
        
      

        FixVector3 pos;

        pos.x = (Fix64) startPos.position.x;
        pos.y = (Fix64) startPos.position.y;
        pos.z = (Fix64) startPos.position.z;
        rc.Pos = pos;
        rc.UpdatePostion();
        rc.UpdateFace();
        
        var e = world.CreateEntity(2,1001);
        e.Camp = 1;

        var eRc = e.GetComponent<RenderComponent>();
        rc.UpdatePostion();
        rc.UpdateFace();
        
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
        
        UIManager.Instance.Show(1);
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
        if(null == entity)
            return;
        FixVector3 dir = FixVector3.Zero;
        Animator ani = entity.GetComponent<RenderComponent>().Animator;
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
