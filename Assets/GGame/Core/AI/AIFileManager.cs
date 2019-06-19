
using System.IO;

namespace GGame.Core
{
    public class AIFileManager : behaviac.FileManager
    {
        public AIFileManager()
        {
            
        }

        public override byte[] FileOpen(string filePath, string ext)
        {
#if CLIENT_LOGIC
            string path = Path.GetFileNameWithoutExtension(filePath);
            return ResourceManager.Instance.LoadBytes(path);
#endif
            return null;
        }

        public override void FileClose(string filePath, string ext, byte[] pBuffer)
        {
           
        }
    }
}