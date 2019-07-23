using GGame.Core;

namespace GGame.Hotfix
{
   
    public sealed class Program 
    {
        public static void Main()
        {
            LogServer.Instance.Debug("Hotfixed start");
            FrameFactory.Instance.Init();
        }
    }
}

