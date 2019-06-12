
using System.IO;
using UnityEditor;
[InitializeOnLoad]
public class HotfixEditor
{
    private const string ScriptAssembliesDir = "Library/ScriptAssemblies/";
    private const string CodeDir = "Assets/GGame/Res/Hotfix/";
    private const string HotfixDll = "GGame.Hotfix.dll";
    private const string HotfixPdb = "GGame.Hotfix.pdb";
    private const string dllPath = "hotfix_dll";
    private const string pdbPath = "hotfix_pdb";
    
    static HotfixEditor()
    {
        string targetDllPath = $"{CodeDir}{dllPath}.bytes";
        string targetPDBPath = $"{CodeDir}{pdbPath}.bytes";
        File.Copy($"{ScriptAssembliesDir}{HotfixDll}", targetDllPath, true);
        File.Copy($"{ScriptAssembliesDir}{HotfixPdb}", targetPDBPath, true);
        
        AssetDatabase.Refresh();
        
        AssetImporter importer = AssetImporter.GetAtPath(targetDllPath);

        importer.assetBundleName = $"hotfix_dll.unity3d";
        
        AssetImporter importer2 = AssetImporter.GetAtPath(targetPDBPath);

        importer2.assetBundleName = $"hotfix_pdb.unity3d";

        AssetDatabase.Refresh();
    }
}