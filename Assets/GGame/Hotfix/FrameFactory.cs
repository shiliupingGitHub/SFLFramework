
using System.Collections.Generic;
using System;
using GGame.Hybird;
using GGame.Hybird.Hotfix;

namespace GGame.Hotfix
{
    
    public class FrameFactory : SingleTon<FrameFactory>
    {
        Dictionary<int,Type> _types = new Dictionary<int,Type>();
        public override void OnInit()
        {
            _types.Clear();
            UIManager.Instance.OnNewFrame += OnCreateFrame;
       
            foreach (var type in HotfixManager.Instance.HotfixType)
            {
                var frameIdAttrs = type.GetCustomAttributes(typeof(FrameIDAttribute), false);

                foreach (var attr in frameIdAttrs)
                {
                    var _attr = attr as FrameIDAttribute;
                    _types[_attr.Id] = type;
                }
            }
            
            
        }

        Frame OnCreateFrame(int id)
        {
            if (_types.TryGetValue(id, out var type))
            {
               return Activator.CreateInstance(type) as Frame;
            }
            return null;
        }
        
    }
}