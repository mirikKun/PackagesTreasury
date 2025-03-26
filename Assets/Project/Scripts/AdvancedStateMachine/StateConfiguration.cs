using System.Collections.Generic;

namespace Project.Scripts.AdvancedStateMachine
{
    public struct StateConfiguration
    {
        public IState State;
        public List<TransitionConfiguration> Transitions;
    }
}