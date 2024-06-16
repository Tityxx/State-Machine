using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tityx.StateMachineSystem.Final
{
    public class StateMachine : IStateMachine, ITickable, IDisposable
    {
        private readonly Dictionary<Type, IExitableState> _states;

        private IExitableState _currentState;

        public IExitableState CurrentState => _currentState;

        public StateMachine()
        {
            _states = new Dictionary<Type, IExitableState>();
        }

        public virtual void Tick()
        {
            if (_currentState != null && _currentState is IUpdateble updatable)
                updatable.Update();
        }

        public virtual void Dispose()
        {
            if (_currentState != null && _currentState is IDisposable disposable)
                disposable.Dispose();
        }

        public void AddState(IExitableState state)
        {
            _states.TryAdd(state.GetType(), state);

            if (state is IInitializable s)
                s.Initialize();
        }

        public void EnterState(IState state)
        {
            _currentState?.Exit();
            _currentState = state;
            state.Enter();
        }

        public void EnterState<TArg>(IStateWithArgument<TArg> state, TArg arg)
        {
            _currentState?.Exit();
            _currentState = state;
            state.Enter(arg);
        }

        public void EnterState(Type stateType)
        {
            if (!_states.TryGetValue(stateType, out var state))
            {
                Debug.LogError($"Can't find state with type '{stateType.Name}'");
                return;
            }
            EnterState(state as IState);
        }

        public void EnterState<TArg>(Type stateType, TArg arg)
        {
            if (!_states.TryGetValue(stateType, out var state))
            {
                Debug.LogError($"Can't find state with type '{stateType.Name}'");
                return;
            }
            EnterState(state as IStateWithArgument<TArg>, arg);
        }

        public void EnterState<TState>() where TState : IState
        {
            EnterState(typeof(TState));
        }

        public void EnterState<TState, TArg>(TArg arg) where TState : IStateWithArgument<TArg>
        {
            EnterState(typeof(TState), arg);
        }

        public void ExitCurrentState()
        {
            _currentState?.Exit();
            _currentState = null;
        }
    }
}