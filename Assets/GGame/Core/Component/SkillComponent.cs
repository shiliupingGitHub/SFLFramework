using System;
using System.Collections.Generic;
using System.Xml;

namespace GGame.Core
{
    public class SkillComponent: Component, IXmlAwake
    {
        Dictionary<int, IJob> _jobs = new Dictionary<int, IJob>();

        public int CurJobId { get; set; } = 0;
        public  void Awake(World world, XmlNode node)
        {


            var childNode = node.FirstChild;

            while (null != childNode)
            {
                switch (childNode.Name)
                {
                    case "Job":
                        JobServer.Instance.AddJobs(childNode, world, Entity, _jobs);
                        break;
                }
                childNode = childNode.NextSibling;
            }
        }

        public void DoJob(int id)
        {
            
            if (_jobs.TryGetValue(id, out var job))
            {
                CurJobId = id;
                job.Schedule(OnFinishJob);
            }
            
        }

        void OnFinishJob()
        {
            CurJobId = 0;

        }

        public void Cancel()
        {
            if (_jobs.TryGetValue(CurJobId, out var job))
            {
                job.Cancel();
            }
            CurJobId = 0;
        }
        
    }
}