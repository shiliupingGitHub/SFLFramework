using GGame.Core;

namespace GGame.Hotfix
{
   
    public sealed class Program 
    {
        public static void Main()
        {
            Log.Debug("Hotfixed start");
            FrameFactory.Instance.Init();
        }
    }
}

