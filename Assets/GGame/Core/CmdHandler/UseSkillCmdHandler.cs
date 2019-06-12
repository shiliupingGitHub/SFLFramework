

namespace GGame.Core
{
    public class UseSkillCmdHandler : CmdHandler<World, Entity, UseSkillCmd>
    {
        protected override void Run(World world, Entity entity, UseSkillCmd a)
        {
            var skillComponent = entity.GetComponent<SkillComponent>();

            if (skillComponent.CurJobId == 0)
            {
                var mc = entity.GetComponent<MoveComponent>();
                skillComponent.DoJob(a.id);
            }
           
        }
    }
}