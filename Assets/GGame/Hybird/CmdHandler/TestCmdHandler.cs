
using GGame.Core;

namespace GGame.Hybird
{
    public class TestCmdHandler : CmdHandler<TestCmd>
    {
        protected override void Run(TestCmd o)
        {
            LogServer.Instance.Debug("testcmd");
        }
    }
}