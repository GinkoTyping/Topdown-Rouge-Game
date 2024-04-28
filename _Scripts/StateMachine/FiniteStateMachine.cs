using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;

namespace Ginko.StateMachineSystem
{
    public class FiniteStateMachine
    {
        public State CurrentState { get; private set; }
        public void Initialize(State state)
        {
            CurrentState = state;
            CurrentState.Enter();
        }
        public void ChangeState(State state)
        {
            CurrentState.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }
    }
}


