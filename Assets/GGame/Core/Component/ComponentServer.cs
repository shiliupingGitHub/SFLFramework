using System;
using System.Collections.Generic;

namespace GGame.Core
{
    public class ComponentServer : ServerAddType<ComponentServer>
    {
        readonly Dictionary<string, Type> _componentTypes = new Dictionary<string, Type>();


        protected override void OnAdd(Type type)
        {
            GGameEnv.Instance.AddAbstractType<Component>(type, _componentTypes);
        }


        
        public Type GetComponentType(string typeName)
        {
            Type ret = null;

            _componentTypes.TryGetValue(typeName, out ret);
            return ret;
        }
    }
}