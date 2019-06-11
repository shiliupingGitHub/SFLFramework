namespace GGame
{
    public class Log
    {
        public static void Debug(string info)
        {
#if !SERVER
            UnityEngine.Debug.Log(info);
#endif
        }

        public static void Error(string info)
        {
#if !SERVER
            UnityEngine.Debug.LogError(info);
#endif
        }
    }
}