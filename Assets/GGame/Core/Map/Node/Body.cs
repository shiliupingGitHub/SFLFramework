using System;
using System.Xml;

namespace GGame.Core
{
    public class Body : IDisposable, MapNode
    {
        public FixVector3 Position
        {
            get; set;
        } = FixVector3.Zero;

        public FixVector3 Center
        {
            get;
            set;
        } = FixVector3.Zero;

        public FixVector3 Size
        {
            get; set;
        } = FixVector3.Zero;

        public Object UseData { get; set; } = null;

        public void Dispose()
        {
            Position = FixVector3.Zero;
            Center = FixVector3.Zero;
            Size = FixVector3.Zero;
            UseData = null;
            ObjectPool.Instance.Recycle(this);
        }

        public void Load(XmlNode node)
        {

            int a = 0;
            int b = a;
        }
    }
}