using NotImplementedException = System.NotImplementedException;

namespace GGame.Core
{
    public class AIServer : SingleTon<AIServer>, IInit
    {
        private AIFileManager fileMgr;
        
        public void Init()
        {
            fileMgr = new AIFileManager();
            behaviac.Workspace.Instance.FilePath = "AI";
            behaviac.Workspace.Instance.FileFormat = behaviac.Workspace.EFileFormat.EFF_xml;
        }
    }
}