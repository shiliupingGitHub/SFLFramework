
using Cinemachine;
using UnityEngine;
using GGame.Core;
using GGame.Hybird;


public class WorldTest : MonoBehaviour
{
    private World _World;
    // Start is called before the first frame update
    public CinemachineVirtualCamera camera;
    public Transform startPos;
    private GPlayer player;
    void Start()
    {
       
        _World = new World(true);
        
        _World.GetSystem<MapSystem>().LoadMap(1001);
        HotfixServer.Instance.Init();
        UISever.Instance.Show("frame_battle", null);

        player = _World.CreatePlayer<GPlayer>(10000001);
        
        player.Cards.Add(1001);
        player.ExploreEntity = _World.CreateEntityWithPos(player.Cards[0], startPos.transform.position.x,
            startPos.transform.position.z);
        player.ExploreEntity.PlayerId = player.Id;
        CameraServer.Instance.SetExPlore(player.ExploreEntity, camera);
        
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
            info.Uuid = player.Id;
            info.Cmd = cmd;
            world?.AddCachCmde(world.FrameIndex +1, info);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            JumpCmd cmd;
            
            CmdInfo info;
            info.Uuid = player.Id;
            info.Cmd = cmd;
            world?.AddCachCmde(world.FrameIndex +1, info);
        }
        
        
    }

    private void OnDestroy()
    {
        _World?.Dispose();
    }
}
