using NotImplementedException = System.NotImplementedException;

namespace GGame
{
    public class UseSkillCmdHandler : CmdHandler<World, Entity, UseSkillCmd>
    {
        protected override void Run(World world, Entity entity, UseSkillCmd a)
        {
            var skillComponent = entity.GetComponent<GSkillComponent>();

            if (!skillComponent.IsLock)
            {
                var mc = entity.GetComponent<MoveComponent>();
                skillComponent.DoJob(a.id);
            }
           
        }
    }
}