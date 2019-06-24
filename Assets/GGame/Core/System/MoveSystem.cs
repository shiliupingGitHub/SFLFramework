
using GGame.Math;
using Microsoft.Xna.Framework;
using RoyT.AStar;
using VelcroPhysics.Collision.RayCast;

namespace GGame.Core
{
    [Interest(typeof(MoveComponent))]
    public class MoveSystem : Core.System
    {
        public override void OnUpdate()
        {

        }

        public override void OnTick()
        {
            foreach (MoveComponent moveComponent in _interestComponents)
            {

                MoveX(moveComponent);
                MoveY(moveComponent);

            }
        }

        void MoveX(MoveComponent moveComponent)
        {

            if (moveComponent.CurJumpLandFrame > 0)
            {
                moveComponent.CurJumpLandFrame--;
                return;
            }

            Fix64 move = moveComponent.Entity.MoveSpeedX * moveComponent.Entity.Face * moveComponent.HSpeed;
            var curPos = moveComponent.Entity.Pos;

            var ret =  World.GetSystem<MapSystem>()._physixWorld
                .RayCast(new Vector2((float)curPos.x, (float)curPos.y), new Vector2((float)(curPos.x + move), (float)curPos.y));

            if (ret.Count == 0)
            {
                curPos.x += move;
                moveComponent.Entity.Pos = curPos;
            }
            else
            {
                int a = 0;
                int b = a;
            }

        }

        void MoveY(MoveComponent moveComponent)
        {
            if(!moveComponent.IsJump )
                return;
            var curPos = moveComponent.Entity.Pos;
            var mapSystem = World.GetSystem<MapSystem>();
            Fix64 y = curPos.y;

            Fix64 tY = y + moveComponent.CurVSpeed;

                if (tY >= 0)
                {
                    if (moveComponent.CurVSpeed > Fix64.Zero)
                    {

                        var ret =  World.GetSystem<MapSystem>()._physixWorld
                            .RayCast(new Vector2((float)curPos.x, (float)curPos.y), new Vector2((float)(curPos.x), (float)tY));

                        if (ret.Count == 0)
                        {
                            curPos.y = tY;
                            moveComponent.Entity.Pos = curPos;
                        }
                        else
                        {
                            moveComponent.CurVSpeed = 0;
                        }

                    }

                    else if (moveComponent.CurVSpeed < Fix64.Zero)
                    {
           

                        var ret =  World.GetSystem<MapSystem>()._physixWorld
                            .RayCast(new Vector2((float)curPos.x, (float)curPos.y), new Vector2((float)(curPos.x), (float)tY));
                        
                        if (ret.Count == 0)
                        {

                            curPos.y = tY;
                            
                            moveComponent.Entity.Pos = curPos;
                            moveComponent.IsJump = true;
                        }
                        else
                        {
                            if (moveComponent.IsJump)
                            {
                                moveComponent.IsJump = false;
                                moveComponent.CurJumpLandFrame = moveComponent.JumpLandFrame;
#if CLIENT_LOGIC
                                RenderComponent renderComponent = moveComponent.Entity.GetComponent<RenderComponent>();
                                
                                renderComponent.Animator?.SetBool("IsJump", false);
#endif
                            }
                        }
                    }
                    
                    moveComponent.CurVSpeed -= moveComponent.Gravity;
                }
                else
                {
                    moveComponent.CurVSpeed = 0;
                }
           
                
        }


    }
}