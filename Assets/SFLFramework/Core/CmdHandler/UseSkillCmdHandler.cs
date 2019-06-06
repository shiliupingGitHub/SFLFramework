using NotImplementedException = System.NotImplementedException;

namespace GGame
{
    public class UseSkillCmdHandler : CmdHandler<World, Entity, UseSkillCmd>
    {
        protected override void Run(World world, Entity entity, UseSkillCmd a)
        {
            var skillComponent = entity.GetComponent<GSkillComponent>();
            
            skillComponent.DoJob(a.id);
        }
    }
}