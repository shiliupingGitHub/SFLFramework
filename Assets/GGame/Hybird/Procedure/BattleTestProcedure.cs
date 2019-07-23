using Cinemachine;
using GGame.Core;
using GGame.Hybird.Hotfix;
using GGame.Math;
using UnityEngine;

namespace GGame.Hybird.Procedure
{
    public class BattleTestProcedure : Procedure<Transform, CinemachineVirtualCamera>
    {
        public World world { get; set; }
        protected override void OnEnter(Transform startPos, CinemachineVirtualCamera camera)
        {
            HotfixManager.Instance.Init();
            
            world = new World(true);
            
            world.GetSystem<MapSystem>() .LoadMap(1001);
            var entity = world.CreateEntityWithPos(10000001,1001, startPos.transform.position.x, startPos.transform.position.y);
            entity.Camp = 0;
        
            var rc = entity.GetComponent<RenderComponent>();
            camera.m_Follow = rc.GameObject.transform;
            camera.m_LookAt = rc.GameObject.transform;
            
            
            rc.UpdatePostion();
            rc.UpdateFace();
            
            
            UISever.Instance.Show(0);

            TestCmd tc;
            
            CmdServer.Instance.ExecuteCmd(tc);
        }           

    }
}