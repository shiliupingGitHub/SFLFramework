using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using GGame;
using GGame.Core;
using GGame.Hybird;
using GGame.Support;
using UnityEngine;

namespace GGame.Hybird
{
    public class HybirdEnv : SingleTon<HybirdEnv>
    {


        public override void OnInit()
        {
            var assembly = typeof(HybirdEnv).Assembly;
            var types = assembly.GetTypes();
            var baseSingltonType = typeof(ISingleTon);
        
            foreach (var type in types)
            {
                if (baseSingltonType.IsAssignableFrom(type) )
                {
                    var supportAttrs = type.GetCustomAttributes(typeof(AutoInitAttribute), false);

                    if (supportAttrs.Length > 0)
                    {
                        var property =  type.BaseType.GetProperty("Instance");
                        var method =  property.GetGetMethod();
                        var o = method.Invoke(null, null) as ISingleTon;
                   
                        o.Init();
                    }
                
                }
            
            }
        }
    }

}

