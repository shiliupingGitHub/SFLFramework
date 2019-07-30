using NPOI.HSSF.Util;
using UnityEngine.Experimental.PlayerLoop;

namespace GGame.Core
{
    public class Fsm
    {
        private IState _state;
        public void Enter<S>(S state) where S : IState
        {
            _state?.Leave();
            _state = state;
            _state?.Enter();
        }

        public void Leave<S>(S state) where S : IState
        {
            _state?.Leave();
            _state = null;
        }

        public virtual void Update()
        {
            _state?.Update();
        }
        
        public void Enter<S, A, B>(S state, A a, B b) where S : IState
        {
            _state?.Leave();
            _state = state;
            _state?.Enter(a, b);
        }
        
        
        
    }
}