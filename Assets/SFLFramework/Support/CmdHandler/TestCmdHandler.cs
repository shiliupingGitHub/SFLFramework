using GGame;

namespace DefaultNamespace
{
    public class TestCmdHandler : CmdHandler<TestCmd>
    {
        protected override void Run(TestCmd o)
        {
            Log.Debug("testcmd");
        }
    }
}