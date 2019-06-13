namespace GGame.Core
{
    public class Log
    {
        public static void Debug(string info)
        {
#if     UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.Log(info);
#endif
        }

        public static void Error(string info)
        {
#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogError(info);
#endif
        }
    }
}