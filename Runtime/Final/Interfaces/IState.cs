using System.Collections;
using UnityEngine;

namespace Tityx.StateMachineSystem.Final
{
    public interface IState : IExitableState
    {
        public void Enter();
    }
}