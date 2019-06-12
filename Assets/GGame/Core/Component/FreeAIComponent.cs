using System.Xml;

namespace GGame.Core
{
    public class FreeAIComponent: Component
    {
        public FreeMoveAIAgent Agent { get; set; }
        public override void Awake(World world, XmlNode node)
        {
            base.Awake(world, node);

            string btName = node.Attributes["name"].Value;
            Agent = ObjectPool.Instance.Fetch<FreeMoveAIAgent>();
            Agent.btload(btName);
            Agent.btsetcurrent(btName);

        }

        public override void Dispose()
        {
            base.Dispose();
            ObjectPool.Instance.Recycle(Agent);
            Agent = null;
        }
    }
}