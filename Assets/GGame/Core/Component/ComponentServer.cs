using System;
using System.Collections.Generic;

namespace GGame.Core
{
    public class ComponentServer : SingleTon<ComponentServer>, IAddType
    {
        readonly Dictionary<string, Type> _componentTypes = new Dictionary<string, Type>();
        public override void OnInit()
        {
            
        }

        public void AddType(Type t)
        {
          GGameEnv.Instance.AddAbstractType<Component>(t, _componentTypes);
        }
        
        public Type GetComponentType(string typeName)
        {
            Type ret = null;

            _componentTypes.TryGetValue(typeName, out ret);
            return ret;
        }
    }
}