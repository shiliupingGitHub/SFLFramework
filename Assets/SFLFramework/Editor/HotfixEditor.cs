
using System.IO;
using UnityEditor;
[InitializeOnLoad]
public class HotfixEditor
{
    private const string ScriptAssembliesDir = "Library/ScriptAssemblies/";
    private const string CodeDir = "Assets/SFLFramework/Resources/Hotfix/";
    private const string HotfixDll = "SFLHotfix.dll";
    private const string HotfixPdb = "SFLHotfix.pdb";

    
    static HotfixEditor()
    {
        File.Copy($"{ScriptAssembliesDir}{HotfixDll}", $"{CodeDir}{HotfixDll}.bytes", true);
        File.Copy($"{ScriptAssembliesDir}{HotfixPdb}", $"{CodeDir}{HotfixPdb}.bytes", true);
        AssetDatabase.Refresh();
    }
}