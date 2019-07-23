
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
                   
                    
                    var fileStream = File.OpenRead(file);

                    var workbook = new XSSFWorkbook(fileStream);

                    List<Dictionary<string, Object>> dataTables = new List<Dictionary<string, Object>>();
                    for (int i = 0; i < workbook.Count; i++)
                    {

                        var sheet = workbook.GetSheetAt(i) as XSSFSheet;

                        string tableName = sheet.SheetName;
                        string jsonPath = $"Assets/GGame/Res/Config/{tableName}.txt";
                        var nameRow = sheet.GetRow(3) as XSSFRow;
                        var desRow = sheet.GetRow(0) as XSSFRow;
                        var typeRow = sheet.GetRow(1) as XSSFRow;
                        var usedRow = sheet.GetRow(2) as XSSFRow;
                        var cellCount = nameRow.LastCellNum;
                        WriteClass(tableName, typeRow, nameRow, desRow, cellCount);
                        for (int j = 4; j <= sheet.LastRowNum; j++)
                        {
                            Dictionary<string, object> t = new Dictionary<string, object>();
                            var dataRow = sheet.GetRow(j) as XSSFRow;

                            for (int k = 0; k < cellCount; k++)
                            {
                                var used = (usedRow.GetCell(k) as XSSFCell).ToString();

                                if (used == "A" || used == "C")
                                {
                                    var cell = dataRow.GetCell(k) as XSSFCell;

                                    var text = cell.ToString();
                                    var name = (nameRow.GetCell(k) as XSSFCell).ToString();
                                    var dataTtype = (typeRow.GetCell(k) as XSSFCell).ToString();
                                    var data = ConvertToObject(dataTtype, text);
                                    t[name] = data;
                                }
                              
                            }

                            if (t.Count > 0)
                            {
                                dataTables.Add(t);
                            }
                           
                            
                        }

                        if (dataTables.Count > 0)
                        {
                            string json = LitJson.JsonMapper.ToJson(dataTables);
                       
                            File.WriteAllText(jsonPath, json);
                        }
                        
                       
                       
                      

                    }
                    fileStream.Dispose();
                }
            }
            
            AssetDatabase.Refresh();
            
        }

        static void WriteClass(string className, XSSFRow typeRow, XSSFRow nameRow, XSSFRow desRow, int count)
        {
            string path = Path.Combine("Assets/GGame/Core/Config", $"{className}.cs");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("namespace GGame.Core");
            sb.AppendLine("{");
            sb.AppendLine($"\tpublic class {className} ");
            sb.AppendLine("\t{");
            for (int i = 0; i < count; i++)
            {
                string type = (typeRow.GetCell(i) as XSSFCell).ToString();
                string name = (nameRow.GetCell(i) as XSSFCell).ToString();
                string des = (desRow.GetCell(i) as XSSFCell).ToString();

                sb.AppendLine($"\t\tpublic {type} {name}; //{des}");
            }

            var keyTypeName = (typeRow.GetCell(0) as XSSFCell).ToString();
            var keyName = (nameRow.GetCell(0) as XSSFCell).ToString();
            sb.AppendLine($"\t\tprivate static Dictionary<{keyTypeName},{className}> _dic;");
            sb.AppendLine($"\t\tpublic static Dictionary<{keyTypeName},{className}> Dic");
            
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tget");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tif(_dic == null)");
            sb.AppendLine("\t\t\t\t{");
            sb.AppendLine("\t\t\t\t\tLoad();");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t\treturn _dic;");
            sb.AppendLine("\t\t\t}");
                
            sb.AppendLine("\t\t}");

            sb.AppendLine("\t\tstatic void Load()");
            
            sb.AppendLine("\t\t{");

            {
                //加载表格

                sb.AppendLine($"\t\t\tvar text = ResourceServer.Instance.LoadText(\"{className}\");");
                sb.AppendLine($"\t\t\tvar listData = LitJson.JsonMapper.ToObject<List<{className}>>(text);");
                sb.AppendLine("\t\t\t_dic = new Dictionary<int, skill_config>();");
                sb.AppendLine("\t\t\tforeach (var data in listData)");
                sb.AppendLine("\t\t\t{");
                sb.AppendLine($"\t\t\t\t_dic[data.{keyName}] = data;");
                sb.AppendLine("\t\t\t}");
            }
            
            sb.AppendLine("\t\t}");
            
            
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            File.WriteAllText(path,sb.ToString());
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