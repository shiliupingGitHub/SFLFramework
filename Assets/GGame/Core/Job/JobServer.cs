using System;
using System.Collections.Generic;

namespace GGame.Core
{
    public class JobServer : SingleTon<JobServer>,IAddType
    {
        readonly Dictionary<string, Type> _jobTypes = new Dictionary<string, Type>();
        public override void OnInit()
        {
            
        }

        public void AddType(Type t)
        {
            GGameEnv.Instance.AddInfeceType<IJob>(t, _jobTypes);
        }
        
        
        public Type GetJobType(string typeName)
        {
            Type ret = null;

            _jobTypes.TryGetValue(typeName, out ret);
            
            return ret;
        }
    }
}