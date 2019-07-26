using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using GGame.Core;
namespace GGame.Hybird
{
    public class HybirdResourceServer  : GResourceServer, IAutoInit
{
    Dictionary<string, string[]> _dependenciesCache = new Dictionary<string, string[]>();
     Dictionary<string, UnityEngine.Object> _objects = new Dictionary<string, Object>();
     Dictionary<string, AssetBundle> _loadedBundles = new Dictionary<string, AssetBundle>();

     HybirdResManifest _manifest = new HybirdResManifest();

    public override string LoadText(string path)
    {

        LoadBundle(path);
        var o = _objects[path] as UnityEngine.TextAsset;
        return o.text;


    }
    
    public override byte[] LoadBytes(string path)
    {

        LoadBundle(path);
        var o = _objects[path] as UnityEngine.TextAsset;
        return o.bytes;

    }
    
    public override GGameObject LoadPrefab(string path)
    {
        LoadBundle(path);
        var o = _objects[path] as UnityEngine.GameObject;
        var go =ObjectServer.Instance.Fetch<HybirdGGameObject>();

        go.GameObject = GameObject.Instantiate(o);
        
        return go;
    }
    

    

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
#else
             if(_loadedBundles.ContainsKey(depend))
                continue;
            string fullPath = null;
            var fullPersistPath = Path.Combine(Application.persistentDataPath, depend);
            if (File.Exists(fullPersistPath))
            {
                fullPath = fullPersistPath;
            }
            else
            {
                var fullStreamPath = Path.Combine(Application.streamingAssetsPath, depend);

                if (File.Exists(fullStreamPath))
                {
                    fullPath = fullStreamPath;
                }
            }

            if (string.IsNullOrEmpty(fullPath))
            {
                LogServer.Instance.Error($"{path} doest not exist");
            }
            else
            {
                var bundle = AssetBundle.LoadFromFile(fullPath);
                var objects = bundle.LoadAllAssets();

                foreach (var o in objects)
                {
                    _objects[o.name] = o;
                }

                _loadedBundles[depend] = bundle;
            }

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
     HybirdRes res = _manifest.Res[assetBundleName];

        dependencies = res.Dependence;
#endif

       
        
        _dependenciesCache.Add(assetBundleName, dependencies);
        return dependencies;
    }
    
    public void Init()
    {
        string path = null;
        var persistPath = Path.Combine(Application.persistentDataPath, "res.manifest");
        
        if (File.Exists(persistPath))
            path = persistPath;
        else
        {
            var streamPath = Path.Combine(Application.streamingAssetsPath, "res.manifest");

            if (File.Exists(streamPath))
            {
                path = streamPath;
            }
            
        }

        if (null != path)
        {
            var txt = File.ReadAllText(path);

            _manifest =  LitJson.JsonMapper.ToObject<HybirdResManifest>(txt);
        }
    }
}

}

