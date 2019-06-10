namespace GGame
{
    public class Log
    {
        public void Debug(string info)
        {
#if !SERVER
            UnityEngine.Debug.Log(info);
#endif
        }

        public void Error(string info)
        {
#if !SERVER
            UnityEngine.Debug.LogError(info);
#endif
        }
    }
}