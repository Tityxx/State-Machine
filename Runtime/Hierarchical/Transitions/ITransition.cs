namespace Tityx.StateMachineSystem.Hierarchical
{
    public interface ITransition
    {
        public IState To { get; }
        public IPredicate Condition { get; }
    }
}