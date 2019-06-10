using System;
using System.Collections.Generic;
using System.Xml;
using NotImplementedException = System.NotImplementedException;

namespace GGame
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
                var type = Enverourment.Instance.GetActionType(childNode.Name);
                if (null != type)
                {
                    var action = ObjectPool.Instance.Fetch(type) as IAction;
                    
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

            _frameNum = Convert.ToInt32(data["frameNum"]?.Value);
        }

        protected override void OnSchedule()
        {
            _curFrame = 0;
#if !SERVER
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
            }

            if (_actions.TryGetValue(_curFrame, out var actions))
            {
                foreach (var action in actions)
                {
                    Enverourment.Instance.ExecuteCmd(_world, _entity, action);
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