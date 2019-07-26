

using System.IO;
using System.Linq;
using GGame.Hybird;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.Build.Pipeline;
using UnityEngine;

namespace GGame.Editor
{
    
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
            
            Build("PC", BuildTarget.StandaloneWindows64);
        }
        
        [MenuItem("Tools/Bundle/buildAndroid")]
        static void BuildBundleAndroid()
        {
            
            Build("Android", BuildTarget.Android);
        }
        
        [MenuItem("Tools/Bundle/buildIOS")]
        static void BuildBundleIOS()
        {
            
            Build("ios", BuildTarget.iOS);
        }
        

        static void Build(string folder, BuildTarget target)
        {
            var bundles = ContentBuildInterface.GenerateAssetBundleBuilds();
            for (var i = 0; i < bundles.Length; i++)
                bundles[i].addressableNames = bundles[i].assetNames.Select(Path.GetFileNameWithoutExtension).ToArray();
        
            var result = CompatibilityBuildPipeline.BuildAssetBundles($"{Application.streamingAssetsPath}/{folder}", BuildAssetBundleOptions.None,
                target);
            var manifest = new HybirdResManifest();

            var createdBundeNames = result.GetAllAssetBundles();

            foreach (var bunde in createdBundeNames)
            {
                HybirdRes res = new HybirdRes();

                res.Name = bunde;

                res.Crc = result.GetAssetBundleCrc(bunde);
                res.Dependence = result.GetAllDependencies(bunde);

                manifest.Res[bunde] = res;
            }

            var resPath = Path.Combine(Application.streamingAssetsPath, folder);
            var infoPath = Path.Combine(resPath, "res.manifest");
            var json = LitJson.JsonMapper.ToJson(manifest);
            File.WriteAllText(infoPath, json);

            if (File.Exists($"{resPath}/{folder}.manifest"))
            {
                File.Delete($"{resPath}/{folder}.manifest");
            }
            
            AssetDatabase.Refresh();
        }
    
    }
}


