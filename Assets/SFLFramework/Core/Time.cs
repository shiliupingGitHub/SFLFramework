
namespace GGame
{
    
    public static class Time 
    
    {
        public static Fix64 DeltaTime
        {
            get { return (Fix64) UnityEngine.Time.deltaTime; }
        }
    }
}

