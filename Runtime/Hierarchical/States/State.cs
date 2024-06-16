using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tityx.StateMachineSystem.Hierarchical
{
    public class State : IState
    {
        protected class Node
        {
            public IState State;
            public HashSet<Transition> Transitions { get; }

            public Node(IState state)
            {
                State = state;
                Transitions = new HashSet<Transition>();
            }

            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));
            }
        }

        protected Node _currNode;
        protected Dictionary<Type, Node> _nodes = new();
        protected HashSet<Transition> _anyTransitions = new();

        protected float _enterTime;

        public float EnterTime => _enterTime;

        public virtual void Enter()
        {
            _enterTime = Time.time;
            _currNode?.State.Enter();
        }

        public virtual void Exit()
        {
            _currNode?.State.Exit();
        }

        public virtual void Update()
        {
            _currNode?.State.Update();

            var transition = GetTransition();
            if (transition != null && transition.To != null)
            {
                ChangeState(transition.To);
                return;
            }
        }

        public virtual void FixedUpdate()
        {
            _currNode?.State.FixedUpdate();
        }

        public void SetState(IState state)
        {
            _currNode = GetNode(state);
            _currNode.State.Enter();
        }

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetNode(from).AddTransition(GetNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition)
        {
            _anyTransitions.Add(new Transition(GetNode(to).State, condition));
        }

        public void ChangeState(IState state)
        {
            if (_currNode.State == state)
                return;

            _currNode.State?.Exit();
            _currNode = GetNode(state);
            _currNode.State?.Enter();
        }

        protected ITransition GetTransition()
        {
            foreach (var t in _anyTransitions)
            {
                if (t.Condition.Evaluate())
                    return t;
            }

            if (_currNode != null)
            {
                foreach (var t in _currNode.Transitions)
                {
                    if (t.Condition.Evaluate())
                        return t;
                }
            }

            return null;
        }

        protected Node GetNode(IState state)
        {
            var node = _nodes.GetValueOrDefault(state.GetType());
            if (node == null)
            {
                node = new Node(state);
                _nodes.Add(state.GetType(), node);
            }
            return node;
        }
    }
}