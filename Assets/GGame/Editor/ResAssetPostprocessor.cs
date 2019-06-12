using System.IO;
using UnityEditor;

namespace GGame.Editor
{
    public class ResAssetPostprocessor : AssetPostprocessor
    {

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (var asset in importedAssets)
            {
                string assetName = Path.GetFileNameWithoutExtension(asset);

                if (asset.StartsWith("Assets/GGame/Res/"))
                {
                    
                    AssetImporter importer = AssetImporter.GetAtPath(asset);

                    importer.assetBundleName = $"{assetName}.unity3d";
                }
                
               
            }
        }
    }
}