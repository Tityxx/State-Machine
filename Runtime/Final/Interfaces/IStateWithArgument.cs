namespace Tityx.StateMachineSystem.Final
{
    public interface IStateWithArgument<TArg> : IExitableState
    {
        public void Enter(TArg arg);
    }
}