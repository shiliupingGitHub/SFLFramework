namespace GGame.Hotfix
{
   
    public sealed class Program 
    {
        public static void Main()
        {
            GGame.Log.Debug("Hotfixed start");
            FrameFactory.Instance.Init();
        }
    }
}

