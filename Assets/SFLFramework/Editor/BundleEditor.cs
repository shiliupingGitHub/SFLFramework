

using UnityEditor;

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
    
}
