using System;
using System.Collections.Generic;

namespace Tityx.StateMachine
{
    public class StateMachine : State
    {
        protected class StateNode
        {
            public IState State;
            public HashSet<Transition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<Transition>();
            }

            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));
            }
        }

        protected StateNode _currNode;
        protected Dictionary<Type, StateNode> _nodes = new();
        protected HashSet<Transition> _anyTransitions = new();

        public override void Enter()
        {
            base.Enter();
            _currNode.State?.Enter();
        }

        public override void Exit()
        {
            _currNode.State?.Exit();
            base.Exit();
        }

        public override void Update()
        {
            base.Enter();
            _currNode.State?.Update();

            var transition = GetTransition();
            if (transition != null)
            {
                ChangeState(transition.To);
                return;
            }
        }

        public override void FixedUpdate()
        {
            base.Enter();
            _currNode.State?.FixedUpdate();
        }

        public void SetState(IState state)
        {
            _currNode = _nodes[state.GetType()];
            _currNode.State?.Enter();
        }

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetNode(from).AddTransition(GetNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition)
        {
            _anyTransitions.Add(new Transition(GetNode(to).State, condition));
        }

        protected ITransition GetTransition()
        {
            foreach (var t in _anyTransitions)
            {
                if (t.Condition.Evaluate())
                    return t;
            }

            foreach (var t in _currNode.Transitions)
            {
                if (t.Condition.Evaluate())
                    return t;
            }

            return null;
        }

        protected void ChangeState(IState state)
        {
            if (_currNode.State == state)
                return;

            _currNode.State?.Exit();
            _currNode = _nodes[state.GetType()];
            _currNode.State?.Enter();
        }

        protected StateNode GetNode(IState state)
        {
            var node = _nodes.GetValueOrDefault(state.GetType());
            if (node == null)
            {
                node = new StateNode(state);
                _nodes.Add(state.GetType(), node);
            }
            return node;
        }
    }
}