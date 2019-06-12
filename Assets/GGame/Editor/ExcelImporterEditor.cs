
using System;
using System.Collections.Generic;
using System.IO;
using NPOI.XSSF.UserModel;
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
                    string tableName = Path.GetFileNameWithoutExtension(file);
                    string jsonPath = $"Assets/GGame/Res/Config/{tableName}.txt";
                    var fileStream = File.OpenRead(file);

                    var workbook = new XSSFWorkbook(fileStream);

                    List<Dictionary<string, Object>> dataTables = new List<Dictionary<string, Object>>();
                    for (int i = 0; i < workbook.Count; i++)
                    {

                        var sheet = workbook.GetSheetAt(i) as XSSFSheet;
                        var nameRow = sheet.GetRow(3) as XSSFRow;
                        var desRow = sheet.GetRow(0) as XSSFRow;
                        var typeRow = sheet.GetRow(1) as XSSFRow;
                        var usedRow = sheet.GetRow(2) as XSSFRow;
                        var cellCount = nameRow.LastCellNum;

                        for (int j = 4; j <= sheet.LastRowNum; j++)
                        {
                            Dictionary<string, object> t = new Dictionary<string, object>();
                            var dataRow = sheet.GetRow(j) as XSSFRow;

                            for (int k = 0; k < cellCount; k++)
                            {
                                var cell = dataRow.GetCell(k) as XSSFCell;

                                var text = cell.ToString();
                                var name = (nameRow.GetCell(k) as XSSFCell).ToString();
                                var dataTtype = (typeRow.GetCell(k) as XSSFCell).ToString();
                                var data = ConvertToObject(dataTtype, text);
                                t[name] = data;
                            }
                            
                            dataTables.Add(t);
                            
                        }

                        string json = LitJson.JsonMapper.ToJson(dataTables);
                       
                       File.WriteAllText(jsonPath, json);
                       
                      

                    }
                    fileStream.Dispose();
                }
            }
            
            AssetDatabase.Refresh();
            
        }

        static  Object ConvertToObject(string szType, string text)
        {
            switch (szType)
            {
                case "int":
                    return System.Convert.ToInt32(text);
            }
            return null;
        }
        
        
    }
}