using UnityEngine;

namespace Tityx.StateMachine
{
    public abstract class State : IState
    {
        protected float _enterTime;

        public float EnterTime => _enterTime;

        public virtual void Enter()
        {
            _enterTime = Time.time;
        }

        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}