using Cinemachine;
using GGame.Core;

namespace GGame.Hybird
{
    public class CameraServer : SingleTon<CameraServer>
    {

        public void SetExPlore(Entity entity, CinemachineVirtualCamera virtualCamera)
        {
            RenderComponent rc = entity.GetComponent<RenderComponent>();

            virtualCamera.Follow = rc.GameObject.transform;
            virtualCamera.LookAt = rc.GameObject.transform;
        }
        
    }
}