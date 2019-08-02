using System.Xml;

namespace GGame.Core
{
#if Client_Logic
    [XLua.LuaCallCSharp]
#endif
    public class FreeAIComponent: Component, IXmlAwake
    {
        public FreeMoveAIAgent Agent { get; set; }
        public  void Awake(World world, XmlNode node)
        {
            
            string btName = node.Attributes["name"].Value;
            Agent = new FreeMoveAIAgent();
            Agent.btload(btName);
            Agent.btsetcurrent(btName);

        }

        public override void Dispose()
        {
            base.Dispose();
            ObjectServer.Instance.Recycle(Agent);
            Agent = null;
        }
    }
}