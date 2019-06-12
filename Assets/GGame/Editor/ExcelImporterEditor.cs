
using System.IO;
using UnityEditor;

namespace GGame.Editor
{
    public class ExcelImporterEditor
    {
        [MenuItem("Tools/Excel/Import")]
        static void ImportExcel()
        {
            string path = "ConfigProject";
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);

                foreach (var file in files)
                {
                    var fileStream = File.OpenRead(file);

                    var workbook = new NPOI.XSSF.UserModel.XSSFWorkbook(fileStream);
                    
                    fileStream.Dispose();
                }
            }
            
        }
        
    }
}