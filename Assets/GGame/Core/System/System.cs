using System;
using System.Collections.Generic;

namespace GGame.Core
{
    public abstract class System : IDisposable
    {
        public World World { get; set; }
        protected List<Component> _interestComponents = new List<Component>();

        public void AddInterest(Component c)
        {
            _interestComponents.Add(c);
        }

        public void RemoveInterest(Component c)
        {
            _interestComponents.Remove(c);
        }
        
        public virtual void Dispose()
        {
            _interestComponents.Clear();
            ObjectServer.Instance.Recycle(this);
        }
    } 

}

