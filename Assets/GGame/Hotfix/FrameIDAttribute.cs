using System;

namespace GGame.Hotfix
{
    public class FrameIDAttribute : Attribute
    {
        private int _Id;
        public int Id
        {
            get { return _Id; }
            
        }

        public FrameIDAttribute(int id)
        {
            _Id = id;
        }
        
    }
}