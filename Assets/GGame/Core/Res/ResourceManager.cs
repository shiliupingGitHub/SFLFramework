using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


using Object = UnityEngine.Object;

namespace GGame.Core
{
    public class ResourceManager : SingleTon<ResourceManager>
{
#if !SERVER   
     Dictionary<string, string[]> _dependenciesCache = new Dictionary<string, string[]>();
     Dictionary<string, UnityEngine.Object> _objects = new Dictionary<string, Object>();
#endif
    public string LoadText(string path)
    {
#if !SERVER
        LoadBundle(path);
        var o = _objects[path] as UnityEngine.TextAsset;
        return o.text;
#else
        return null;
#endif

    }
    
    public byte[] LoadBytes(string path)
    {
#if !SERVER
        LoadBundle(path);
        var o = _objects[path] as UnityEngine.TextAsset;
        return o.bytes;
#else
        return null;
#endif

    }
    
#if !SERVER
    public UnityEngine.GameObject LoadPrefab(string path)
    {
        LoadBundle(path);
        var o = _objects[path] as UnityEngine.GameObject;
        return o;
    }
    
#endif

#if !SERVER

    void LoadBundle(string path)
    {
        path = $"{path}.unity3d";
        string[] depends = GetSortedDependencies(path);

        foreach (var depend in depends)
        {
#if UNITY_EDITOR
            var assetPaths =  UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(depend);

            foreach (var assetPath in assetPaths)
            {
                string assetName = Path.GetFileNameWithoutExtension(assetPath);
                
                
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                _objects[assetName] = asset;
                
            }
            
            return;
#endif
        }
    }
    string[] GetSortedDependencies(string assetBundleName)
    {
        Dictionary<string, int> info = new Dictionary<string, int>();
        List<string> parents = new List<string>();
        CollectDependencies(parents, assetBundleName, info);
        string[] ss = info.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
        return ss;
    }
    
    void CollectDependencies(List<string> parents, string assetBundleName, Dictionary<string, int> info)
    {
        parents.Add(assetBundleName);
        string[] deps = GetDependencies(assetBundleName);
        foreach (string parent in parents)
        {
            if (!info.ContainsKey(parent))
            {
                info[parent] = 0;
            }
            info[parent] += deps.Length;
        }


        foreach (string dep in deps)
        {
            if (parents.Contains(dep))
            {
                throw new Exception($"包有循环依赖，请重新标记: {assetBundleName} {dep}");
            }
            CollectDependencies(parents, dep, info);
        }
        parents.RemoveAt(parents.Count - 1);
    }
    
    string[] GetDependencies(string assetBundleName)
    {
        string[] dependencies = new string[0];
        if (_dependenciesCache.TryGetValue(assetBundleName,out dependencies))
        {
            return dependencies;
        }

#if UNITY_EDITOR
        dependencies = UnityEditor.AssetDatabase.GetAssetBundleDependencies(assetBundleName, true);
#else

#endif
        
        _dependenciesCache.Add(assetBundleName, dependencies);
        return dependencies;
    }
#endif

    public override void OnInit()
    {
       
    }
}

}

