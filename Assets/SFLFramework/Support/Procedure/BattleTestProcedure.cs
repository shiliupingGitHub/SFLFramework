using Cinemachine;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace GGame.Support
{
    public class BattleTestProcedure : Procedure<Transform, CinemachineVirtualCamera>
    {
        public World world { get; set; }
        protected override void OnEnter(Transform startPos, CinemachineVirtualCamera camera)
        {
            HotfixManager.Instance.Init();
            
            world = new World();

            var entity = world.CreateEntity(1,1001);
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
            
            UIManager.Instance.Show(0);
        }           

    }
}