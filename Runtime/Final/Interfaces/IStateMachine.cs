using System;

namespace Tityx.StateMachineSystem.Final
{
    public interface IStateMachine
    {
        public IExitableState CurrentState { get; }

        public void AddState(IExitableState state);
        public void EnterState(IState state);
        public void EnterState<TArg>(IStateWithArgument<TArg> state, TArg arg);
        public void EnterState(Type stateType);
        public void EnterState<TArg>(Type stateType, TArg arg);
        public void EnterState<TState>() where TState : IState;
        public void EnterState<TState, TArg>(TArg arg) where TState : IStateWithArgument<TArg>;
        public void ExitCurrentState();
    }
}