using System;
using System.Collections.Generic;
using System.Xml;
namespace GGame.Core
{
    public class AnimationJob : Job
    {
        Dictionary<int, List<IAction>> _actions = new Dictionary<int, List<IAction>>();
        private int _curFrame = 0;
        private int _frameNum = 0;
        protected override void OnInit(XmlNode data)
        {
            var childNode = data.FirstChild;

            while (null != childNode)
            {
                var type = ActionServer.Instance.GetActionType(childNode.Name);
                if (null != type)
                {
                    var action = ObjectServer.Instance.Fetch(type) as IAction;
                    
                    action.Init(childNode);

                    List<IAction> cacheAction = null;

                    if (!_actions.TryGetValue(action.FrameIndex, out cacheAction))
                    {
                        cacheAction = new List<IAction>();

                        _actions[action.FrameIndex] = cacheAction;
                    }
                    
                    cacheAction.Add(action);
                    
                }
                
                childNode = childNode.NextSibling;
            }

            _frameNum = Convert.ToInt32(data.Attributes["frameNum"]?.Value);
        }

        protected override void OnSchedule()
        {
            _curFrame = 0;
#if CLIENT_LOGIC
            var rc = _entity.GetComponent<RenderComponent>();
            
            rc.Animator.SetTrigger("Skill");
#endif
        }

        public override void Tick()
        {
            base.Tick();

            if (_curFrame > _frameNum)
            {
                Finish();
                return;
            }

            if (_actions.TryGetValue(_curFrame, out var actions))
            {
                foreach (var action in actions)
                {
                    CmdServer.Instance.ExecuteCmd(_world, _entity, action);
                }
            }

            _curFrame++;

        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var actions in _actions)
            {
                foreach (var action in actions.Value)
                {
                    action.Dispose();
                }
            }
            
            _actions.Clear();
            _curFrame = 0;
            _frameNum = 0;
        }
    }
}