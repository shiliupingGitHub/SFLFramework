
using GGame.Math;
using RoyT.AStar;

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
            Fix64 move = moveComponent.Entity.MoveSpeedX * moveComponent.Entity.Face * moveComponent.HSpeed;
            var curPos = moveComponent.Entity.Pos;

            if (move != Fix64.Zero)
            {
                Fix64 x = curPos.x;
                Fix64 t_x = x + move;
                int y = (int) curPos.y;

                if (move > Fix64.Zero)
                {
                    int min =(int) global::System.Math.Floor((float)x);
                    int max =(int) global::System.Math.Ceiling((float) t_x);
                    
                    for (int i = min + 1; i <= max; i++)
                    {
                        float cost = World.Map.Grid.GetCellCost(new Position((int) i, y));

                        if (cost > 5.0f)
                        {
                            max--;
                            break;
                        }
                    }

                    if (max != min)
                    {
                        curPos.x = JMath.Min(t_x, max);
                        moveComponent.Entity.Pos = curPos;
                    }
                    
                }

                else
                {
                    int min = (int)global::System.Math.Floor((float) t_x);
                    int max = (int) global::System.Math.Ceiling((float) x);

                    for (int i = max - 1; i >= min; i--)
                    {
                        float cost = World.Map.Grid.GetCellCost(new Position((int) i, y));

                        if (cost > 5.0f)
                        {
                            min++;
                            break;
                        }
                    }

                    if (min != max)
                    {
                        curPos.x = JMath.Max(t_x, min);
                        moveComponent.Entity.Pos = curPos;
                    }
                    
                }
                    
                

               
            }
        }

        void MoveY(MoveComponent moveComponent)
        {
            var curPos = moveComponent.Entity.Pos;
            Fix64 y = curPos.y;
            int x = (int) curPos.x;

                Fix64 tY = y + moveComponent.CurVSpeed;

                if (tY >= 0)
                {
                    if (moveComponent.CurVSpeed > Fix64.Zero)
                    {
                        moveComponent.IsJump = true;
                        int down = (int) global::System.Math.Floor((float)y);
                        int up = (int) global::System.Math.Ceiling((float) tY);
                
                        for (int i = down + 1; i <= up; i++)
                        {
                            float cost = World.Map.Grid.GetCellCost(new Position((int) x, i));

                            if (cost > 5.0f)
                            {
                                up = i - 1;
                                break;
                            }
                    
                        }

                        if (up != down)
                        {
                            curPos.y = JMath.Min(tY, up);
                            moveComponent.Entity.Pos = curPos;
                        }
                    }

                    else if (moveComponent.CurVSpeed < Fix64.Zero)
                    {
                        int up = (int) global::System.Math.Ceiling((float)y);
                        int down = (int) global::System.Math.Floor((float) tY);

                        for (int i = up - 1; i >= down; i--)
                        {
                            float cost = World.Map.Grid.GetCellCost(new Position((int) x, i));

                            if (cost > 5.0f)
                            {
                                down = i + 1;
                                break;
                            }
                        }
                        
                        if (up != down)
                        {
                            
                            curPos.y = JMath.Max(tY, down);

                            if (down >= tY)
                            {
                                if (moveComponent.IsJump)
                                {
                                    moveComponent.IsJump = false;
#if CLIENT_LOGIC
                                    RenderComponent renderComponent = moveComponent.Entity.GetComponent<RenderComponent>();
                                
                                    renderComponent.Animator?.SetBool("IsJump", false);
#endif
                                }
                            }
                            moveComponent.Entity.Pos = curPos;
                            moveComponent.IsJump = true;
                        }
                        else
                        {
                            if (moveComponent.IsJump)
                            {
                                moveComponent.IsJump = false;
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