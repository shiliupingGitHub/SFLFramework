namespace GGame.Core
{
    public class Log
    {
        public static void Debug(string info)
        {
#if     CLIENT_LOGIC
            UnityEngine.Debug.Log(info);
#endif
        }

        public static void Error(string info)
        {
#if CLIENT_LOGIC
            UnityEngine.Debug.LogError(info);
#endif
        }
    }
}