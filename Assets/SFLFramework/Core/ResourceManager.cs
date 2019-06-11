using System.Collections;
using System.Collections.Generic;
using GGame;
using UnityEngine;

public class ResourceManager : SingleTon<ResourceManager>
{

    public string LoadText(string path)
    {
#if !SERVER
        return Resources.Load<TextAsset>(path).text;
#else
        return null;
#endif

    }
    
    public byte[] LoadBytes(string path)
    {
#if !SERVER
        return Resources.Load<TextAsset>(path).bytes;
#else
        return null;
#endif

    }
    
#if !SERVER
    public GameObject LoadPrefab(string path)
    {
        return Resources.Load<GameObject>(path);
    }
    
#endif


    public override void OnInit()
    {
        throw new System.NotImplementedException();
    }
}
