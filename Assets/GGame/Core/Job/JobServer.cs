using System;
using System.Collections.Generic;
using System.Xml;

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
        
        
        public void AddJobs(XmlNode xmlNode, World world, Entity entity, Dictionary<int, IJob> jobs)
        {
            IJob job = null;
            var childNode = xmlNode.FirstChild;

            while (null != childNode)
            {
                var jobType = JobServer.Instance.GetJobType(childNode.Name);
                var temp = ObjectServer.Instance.Fetch(jobType) as IJob;

                if (null != temp)
                {
                    temp.Init(world, entity, childNode);
                    if (null == job)
                        job = temp;
                    else
                    {
                        job.AddChild(temp);
                    }
                    
                }
                
                childNode = childNode.NextSibling;
            }

            if (null != job)
            {
                int id = Convert.ToInt32(xmlNode.Attributes["id"].Value);
                jobs[id] = job;
            }
               
        }
    }
}