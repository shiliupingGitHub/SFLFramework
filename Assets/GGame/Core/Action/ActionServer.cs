using System;
using System.Collections.Generic;

namespace GGame.Core
{
    public class ActionServer : ServerAddType<ActionServer>
    {
        readonly Dictionary<string, Type> _actionTypes = new Dictionary<string, Type>();

        
        public Type GetActionType(string typeName)
        {
            Type ret = null;

            _actionTypes.TryGetValue(typeName, out ret);
            
            return ret;
        }

        protected override void OnAdd(Type type)
        {
            GGameEnv.Instance.AddInfeceType<IAction>(type, _actionTypes);
        }
    }
}