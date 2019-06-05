using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using GGame;

public class WorldTest : MonoBehaviour
{
    // Start is called before the first frame update
    private World world;
    void Start()
    {
        Enverourment.Instance.Init();
        world = new World();

        world.CreateEntity(1,1001);
        StartTick();
    }

    async void StartTick()
    {
        while (null != world)
        {
            world.Tick();
            await Task.Delay(33);
        }
    }
    // Update is called once per frame
    void Update()
    {
        world.Update();
    }

    private void OnDestroy()
    {
        world?.Dispose();
        world = null;
    }
}
