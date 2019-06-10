using System;
using System.Collections.Generic;
using System.Xml;

namespace GGame
{
    public class GSkillComponent: Component
    {
        Dictionary<int, IJob> _jobs = new Dictionary<int, IJob>();
        public bool IsLock { get; set; } = false;
        public override void Awake(World world, XmlNode node)
        {
            base.Awake(world, node);

            var childNode = node.FirstChild;

            while (null != childNode)
            {
                switch (childNode.Name)
                {
                    case "Job":
                        AddJobs(childNode);
                        break;
                }
                childNode = childNode.NextSibling;
            }
        }

        public void DoJob(int id)
        {
            IsLock = true;
            Entity.GetComponent<MoveComponent>().IsLock = true;
            if (_jobs.TryGetValue(id, out var job))
            {
                job.Schedule(OnFinishJob);
            }
            
        }

        void OnFinishJob()
        {
            IsLock = false;
            Entity.GetComponent<MoveComponent>().IsLock = false;
            
        }
        

        void AddJobs(XmlNode xmlNode)
        {
            IJob job = null;
            var childNode = xmlNode.FirstChild;

            while (null != childNode)
            {
                var jobType = Enverourment.Instance.GetJobType(childNode.Name);
                var temp = ObjectPool.Instance.Fetch(jobType) as IJob;

                if (null != temp)
                {
                    temp.Init(_world, Entity, childNode);
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
                _jobs[id] = job;
            }
               
        }
    }
}