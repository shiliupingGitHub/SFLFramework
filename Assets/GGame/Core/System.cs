using System;
using System.Collections.Generic;

namespace GGame.Core
{
    public abstract class System : IDisposable
    {
        
        protected List<Component> _interestComponents = new List<Component>();

        public void AddInterest(Component c)
        {
            _interestComponents.Add(c);
        }

        public void RemoveInterest(Component c)
        {
            _interestComponents.Remove(c);
        }
        
        public abstract void OnUpdate();
        public abstract void OnTick();

        public virtual void Dispose()
        {
            _interestComponents.Clear();
            ObjectPool.Instance.Recycle(this);
        }
    } 

}

