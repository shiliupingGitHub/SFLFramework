using System.Collections;
using System.Collections.Generic;
using GGame;
using UnityEngine;

public class ResourceManager : SingleTon<ResourceManager>
{

    public string LoadEntityConfig(string path)
    {
       return Resources.Load<TextAsset>(path).text;
    }
#if !SERVER
    public GameObject LoadEntityPrefab(string path)
    {
        return Resources.Load<GameObject>(path);
    }
    
#endif

    
}
