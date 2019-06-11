
using System.IO;
using UnityEditor;
[InitializeOnLoad]
public class HotfixEditor
{
    private const string ScriptAssembliesDir = "Library/ScriptAssemblies/";
    private const string CodeDir = "Assets/GGame/Res/Hotfix/";
    private const string HotfixDll = "GGame.Hotfix.dll";
    private const string HotfixPdb = "GGame.Hotfix.pdb";

    
    static HotfixEditor()
    {
        File.Copy($"{ScriptAssembliesDir}{HotfixDll}", $"{CodeDir}{HotfixDll.ToLower()}.bytes", true);
        File.Copy($"{ScriptAssembliesDir}{HotfixPdb}", $"{CodeDir}{HotfixPdb.ToLower()}.bytes", true);
        
        AssetDatabase.Refresh();
    }
}