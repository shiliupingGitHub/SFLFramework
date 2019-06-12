using NotImplementedException = System.NotImplementedException;

namespace GGame.Core
{
    public class AIManager : SingleTon<AIManager>
    {
        private AIFileManager fileMgr;
        public override void OnInit()
        {
            fileMgr = new AIFileManager();
            behaviac.Workspace.Instance.FilePath = "AI";
            behaviac.Workspace.Instance.FileFormat = behaviac.Workspace.EFileFormat.EFF_xml;
        }
    }
}