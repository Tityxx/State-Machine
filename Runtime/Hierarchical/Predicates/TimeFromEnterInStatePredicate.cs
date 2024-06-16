using UnityEngine;

namespace Tityx.StateMachineSystem.Hierarchical
{
    public class TimeFromEnterInStatePredicate : IPredicate
    {
        private readonly IState _state;
        private readonly float _duration;

        public TimeFromEnterInStatePredicate(IState state, float duration)
        {
            _state = state;
            _duration = duration;
        }

        public bool Evaluate()
        {
            return Time.time - _state.EnterTime >= _duration;
        }
    }
}