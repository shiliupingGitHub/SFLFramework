
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
#region x

                var curPos = moveComponent.Entity.Pos;
                Fix64 x = curPos.x;

                x += moveComponent.Entity.MoveSpeedX * moveComponent.Entity.Face;
                
                curPos.x = x ;

                moveComponent.Entity.Pos = curPos;

#endregion


#region y

                Fix64 y = curPos.y;

                Fix64 tY = y + moveComponent.CurVSpeed;

                if (tY >= 0)
                {
                    if (moveComponent.CurVSpeed > Fix64.Zero)
                    {
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
                            curPos.y = tY;
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
                            moveComponent.Entity.Pos = curPos;
                        }
                    }
                    
                    moveComponent.CurVSpeed -= moveComponent.Gravity;
                }
                else
                {
                    moveComponent.CurVSpeed = 0;
                }
           

#endregion

            }
        }
        
    }
}