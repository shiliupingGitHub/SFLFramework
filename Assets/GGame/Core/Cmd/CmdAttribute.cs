

using System;

namespace GGame.Core
{
   
    public class CmdAttribute : Attribute
    {
        private string _op;

        public string Op
        {
            get => _op;
        }

        public CmdAttribute(string op)
        {
            this._op = op;
        }
   
        
    }
}

