using Cinemachine;
using GGame.Core;
using GGame.Hybird.Hotfix;
using GGame.Math;
using Jitter.LinearMath;
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
            
            var entity = world.CreateEntity(10000001,1001);
            entity.Camp = 0;
        
            var rc = entity.GetComponent<RenderComponent>();
            camera.m_Follow = rc.GameObject.transform;
            camera.m_LookAt = rc.GameObject.transform;
            
            JVector pos;

            pos.X = (Fix64) startPos.position.x;
            pos.Y = (Fix64) startPos.position.y;
            pos.Z = (Fix64) startPos.position.z;
            rc.Entity.Pos = pos;
            rc.Entity.Euler = new JVector(0, -Fix64.PI * 0.5f , 0);
            rc.UpdatePostion();
            rc.UpdateFace();
        
            var e = world.CreateEntity(10000002,1001);
            e.Camp = 1;

            var eRc = e.GetComponent<RenderComponent>();
            rc.UpdatePostion();
            rc.UpdateFace();
            
            UIManager.Instance.Show(0);

            TestCmd tc;
            
            GGameEnv.Instance.ExecuteCmd(tc);
        }           

    }
}