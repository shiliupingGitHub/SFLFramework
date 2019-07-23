using System;
using System.Collections.Generic;

namespace GGame.Core
{
    public class ActionServer : SingleTon<ActionServer>, IAddType
    {
        readonly Dictionary<string, Type> _actionTypes = new Dictionary<string, Type>();
        public override void OnInit()
        {
            
        }

        public void AddType(Type t)
        {
           GGameEnv.Instance.AddInfeceType<IAction>(t, _actionTypes);
        }
        
        public Type GetActionType(string typeName)
        {
            Type ret = null;

            _actionTypes.TryGetValue(typeName, out ret);
            
            return ret;
        }
    }
}