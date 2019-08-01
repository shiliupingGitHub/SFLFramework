
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

            string path = Path.GetFileNameWithoutExtension(filePath);
            return GResourceServer.Instance.Load<byte[]>(path) as byte[];
            
        }

        public override void FileClose(string filePath, string ext, byte[] pBuffer)
        {
           
        }
    }
}