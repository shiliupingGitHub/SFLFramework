using System.Collections.Generic;
using GGame.Math;


namespace GGame.Core
{
    public struct Hurt
    {
        public Entity _Entity;
        public HurtAction _HurtAction;

    };
    [Interest(typeof(HurtedComponent))]
    public class HurtedSystem : Core.System
    {
        private List<Hurt> _list = new List<Hurt>();
        public void AddHurt( Hurt hurt)
        {
            _list.Add(hurt);
        }
        
        public override void OnUpdate()
        {
            
        }

        public override void OnTick()
        {
            foreach (var hurt in _list)
            {
                foreach (HurtedComponent hc in _interestComponents)
                {

                    if (hc.Entity != hurt._Entity && hc.Entity.Camp != hurt._Entity.Camp)
                    {
                        HurtAction ha = hurt._HurtAction;
                        Fix64 distance = ha.Distance;
                        var rc = hurt._Entity.GetComponent<RenderComponent>();
                        var rc_me = hc.Entity.GetComponent<RenderComponent>();
                        var e2e = rc_me.Pos - rc.Pos;
                        var d = FixVector3.Distance(rc.Pos, rc_me.Pos);
                        
                        if (d < distance)
                        {
                            var n1 = e2e.GetNormalized();
                            var n2 = rc.Face.GetNormalized();

                            if (n1.x * n2.x > 0)
                            {
                                var skillMe = hc.Entity.GetComponent<SkillComponent>();

                                skillMe?.Cancel();
#if UNITY_2017_1_OR_NEWER
                                rc_me.Animator.SetTrigger("Hurted");
#endif
                                var ctr = World.Controller;

                                if (null != ctr.OnHurt)
                                {
                                    HurtData hd;
                                    hd.Hp = 20;
                                    ctr.OnHurt(hurt._Entity, hc.Entity, hd);
                                }
                            }
                            

                        }
                        
                    }
                    
                }
            }
            _list.Clear();
        }
    }
}