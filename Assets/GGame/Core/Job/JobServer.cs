using System;
using System.Collections.Generic;

namespace GGame.Core
{
    public class JobServer : ServerAddType<JobServer>
    {
        readonly Dictionary<string, Type> _jobTypes = new Dictionary<string, Type>();
        
        protected override void OnAdd(Type type)
        {
            GGameEnv.Instance.AddInfeceType<IJob>(type, _jobTypes);
        }
        
        public Type GetJobType(string typeName)
        {
            Type ret = null;

            _jobTypes.TryGetValue(typeName, out ret);
            
            return ret;
        }
    }
}