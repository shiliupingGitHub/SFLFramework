using System.IO;
using UnityEditor;

namespace GGame.Editor
{
    public class ResAssetPostprocessor : AssetPostprocessor
    {
        
        private const string ScriptAssembliesDir = "Library/ScriptAssemblies/";
        private const string CodeDir = "Assets/GGame/Res/Hotfix/";
        private const string HotfixDll = "GGame.Hotfix.dll";
        private const string HotfixPdb = "GGame.Hotfix.pdb";
        private const string dllPath = "hotfix_dll";
        private const string pdbPath = "hotfix_pdb";
        
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            bool hotfixcode_changed = false;
            foreach (var asset in importedAssets)
            {
                string assetName = Path.GetFileNameWithoutExtension(asset);
                
                
                if (asset.StartsWith("Assets/GGame/Res/"))
                {
                    if (!asset.Contains(".meta") && File.Exists(asset))
                    {
                        AssetImporter importer = AssetImporter.GetAtPath(asset);

                        importer.assetBundleName = $"{assetName}.unity3d";
                        
                    }
                }

                if (asset.Contains(".cs") && asset.StartsWith("Assets/GGame/Hotfix"))
                {
                    hotfixcode_changed = true;
                }

            }

            if (hotfixcode_changed)
            {
                if (!Directory.Exists(CodeDir))
                    Directory.CreateDirectory(CodeDir);
                string targetDllPath = $"{CodeDir}{dllPath}.bytes";
                string targetPDBPath = $"{CodeDir}{pdbPath}.bytes";
                File.Copy($"{ScriptAssembliesDir}{HotfixDll}", targetDllPath, true);
                File.Copy($"{ScriptAssembliesDir}{HotfixPdb}", targetPDBPath, true);
            }
            

            
        }
    }
}