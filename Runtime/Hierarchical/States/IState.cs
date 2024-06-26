using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tityx.StateMachineSystem.Hierarchical
{
    public interface IState
    {
        public float EnterTime { get; }

        public void Enter();
        public void Exit();
        public void Update();
        public void FixedUpdate();
    }
}