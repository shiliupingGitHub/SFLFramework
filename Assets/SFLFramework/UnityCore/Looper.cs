using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looper : MonoBehaviour
{
    public Action LoopAction;

    // Update is called once per frame
    void Update()
    {
        LoopAction?.Invoke();
    }
}
