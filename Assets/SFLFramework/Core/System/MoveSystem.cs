
namespace GGame
{
    [Interest(typeof(MoveComponent))]
    public class MoveSystem : System
    {
        public override void OnUpdate()
        {
#if !SERVER
            foreach (MoveComponent mc in _interestComponents)
            {
                var rc = mc.Entity.GetComponent<RenderComponent>();

                if (null != rc)
                {
                    var pos = rc.Pos;
                    var dir = mc.Dir;
                    var speed = mc.Speed;
                    pos += dir * speed ;

                    UnityEngine.Vector3 gFinalPos = new UnityEngine.Vector3((float)pos.X, (float)pos.Y, (float)pos.Z);
                    
                    
                    var gPos = rc.GameObject.transform.position;

                    gPos = UnityEngine.Vector3.Lerp(gPos, gFinalPos, UnityEngine.Time.deltaTime > 1.0f ? 1.0f : UnityEngine.Time.deltaTime);

                    rc.GameObject.transform.position = gPos;

                }
                
            }
#endif
        }

        public override void OnTick()
        {
            foreach (MoveComponent mc in _interestComponents)
            {
                var rc = mc.Entity.GetComponent<RenderComponent>();

                if (null != rc)
                {
                    var pos = rc.Pos;
                    var dir = mc.Dir;
                    var speed = mc.Speed;

                    pos += dir * speed;

                    rc.Pos = pos;

                }
                
            }
        }
    }
}