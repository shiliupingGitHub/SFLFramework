using System;

namespace GGame.Core
{
    public class InterestAttribute : Attribute
    {
        private Type _Interest;
        public Type Interest
        {
            get { return _Interest; }
        }
        public InterestAttribute(Type t)
        {
            _Interest = t;
        }
    }
   
    
}

