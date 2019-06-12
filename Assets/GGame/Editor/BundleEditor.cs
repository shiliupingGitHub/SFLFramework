

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.Build.Pipeline;
using UnityEngine;
using UnityEngine.Build.Pipeline;

public class BundleEditor 
{
    [MenuItem("Assets/Bundle/BundleName")]
    static void MakeBundleName()
    {
        foreach (var o in Selection.objects)
        {
            AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(o));

            importer.assetBundleName = $"{o.name}.unity3d";

            AssetDatabase.Refresh();
        }
    }

    [MenuItem("Tools/Bundle/buildPC")]
    static void BuildBundlePc()
    {
        var bundles = ContentBuildInterface.GenerateAssetBundleBuilds();
        for (var i = 0; i < bundles.Length; i++)
            bundles[i].addressableNames = bundles[i].assetNames.Select(Path.GetFileNameWithoutExtension).ToArray();
        
        var result = CompatibilityBuildPipeline.BuildAssetBundles($"{Application.streamingAssetsPath}/PC", BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows64);
        var infoPath = Path.Combine(Application.streamingAssetsPath, "PC/Vertion.info");
        var json = JsonUtility.ToJson(result);
        
        File.WriteAllText(infoPath, json);
        AssetDatabase.Refresh();
        
        
        
    }
    
}
