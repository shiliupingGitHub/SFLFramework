
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
            
            if(moveComponent.Entity.MoveSpeedX == Fix64.Zero)
                return;
            Fix64 move = moveComponent.Entity.MoveSpeedX * moveComponent.Entity.Face * moveComponent.HSpeed;
            var curPos = moveComponent.Entity.Pos;
            FixVector2 outPoint;
            if (!World.GetSystem<MapSystem>().GetHitPoint(new FixVector2((float)curPos.x, (float)curPos.y), new FixVector2((float)(curPos.x + move ), (float)curPos.y), out outPoint))
            {
                curPos.x += move;
                moveComponent.Entity.Pos = curPos;
            }
            else
            {
                

                outPoint.x -= moveComponent.Entity.Face * 0.01f;
                moveComponent.Entity.Pos = outPoint;
            }
            
        }

        void MoveY(MoveComponent moveComponent)
        {
            var curPos = moveComponent.Entity.Pos;
            var mapSystem = World.GetSystem<MapSystem>();
            Fix64 y = curPos.y;

            Fix64 tY = y + moveComponent.CurVSpeed;

                if (tY >= 0)
                {
                    if (moveComponent.CurVSpeed > Fix64.Zero)
                    {

                    
                        FixVector2 outPoint;
                        if (!mapSystem.GetHitPoint(new FixVector2((float)curPos.x, (float)curPos.y), new FixVector2((float)(curPos.x), (float)tY), out outPoint))
                        {
                            curPos.y = tY;
                            moveComponent.Entity.Pos = curPos;
                        }
                        else
                        {
                            
                            moveComponent.Entity.Pos = outPoint;
                            moveComponent.CurVSpeed = 0;
                        }

                    }

                    else if (moveComponent.CurVSpeed < Fix64.Zero)
                    {

                        FixVector2 outPoint;
                        
                        if (!mapSystem.GetHitPoint(new FixVector2((float)curPos.x, (float)curPos.y), new FixVector2((float)(curPos.x), (float) tY ), out outPoint))
                        {

                            curPos.y = tY;
                            
                            moveComponent.Entity.Pos = curPos;
                            moveComponent.IsJump = true;
                        }
                        else
                        {
                            outPoint.y += 0.01f;
                            curPos = outPoint;
                            moveComponent.Entity.Pos = curPos;
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