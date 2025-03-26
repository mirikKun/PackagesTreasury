using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Scripts.AdvancedStateMachine
{
    public class StateMachine {
        private StateNode currentNode;
        private readonly Dictionary<Type, StateNode> nodes = new();
        private readonly HashSet<Transition> anyTransitions = new();
        
        public List<IState> PreviousStates { get; private set; } = new List<IState>();
        public IState CurrentState => currentNode.State;
        private int _maxHistoryStates = 10;

        public void Update() {
            var transition = GetTransition();

            if (transition != null) {
                Debug.Log($"Transitioning from {currentNode.State.GetType().Name} to {transition.To.GetType().Name}");
                ChangeState(transition.To);
                foreach (var node in nodes.Values) {
                    ResetActionPredicateFlags(node.Transitions);
                }
                ResetActionPredicateFlags(anyTransitions);
            }

            currentNode.State?.Update();
            
        }

        private static void ResetActionPredicateFlags(IEnumerable<Transition> transitions) {
            foreach (var transition in transitions.OfType<Transition<ActionPredicate>>()) {
                transition.condition.flag = false;
            }
        }
        
        public void FixedUpdate() {
            currentNode.State?.FixedUpdate();
        }

        public void SetState(IState state) {
            currentNode = nodes[state.GetType()];
            currentNode.State?.OnEnter();
            PreviousStates.Add(state);
        }

        void ChangeState(IState state) {
            if (state == currentNode.State)
                return;

            var previousState = currentNode.State;
            var nextState = nodes[state.GetType()].State;

            previousState?.OnExit();
            nextState.OnEnter();
            AddStateToHistory(state);
            currentNode = nodes[state.GetType()];
        }

        private void AddStateToHistory(IState state)
        {
            PreviousStates.Add(state);
            if (PreviousStates.Count > _maxHistoryStates)
                PreviousStates.RemoveAt(0);
        }

        public void AddTransition<T>(IState from, IState to, T condition) {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition<T>(IState to, T condition) {
            anyTransitions.Add(new Transition<T>(GetOrAddNode(to).State, condition));
        }

        private Transition GetTransition() {
            foreach (var transition in anyTransitions)
                if (transition.Evaluate())
                    return transition;

            foreach (var transition in currentNode.Transitions) {
                if (transition.Evaluate())
                    return transition;
            }

            return null;
        }

        private StateNode GetOrAddNode(IState state) {
            var node = nodes.GetValueOrDefault(state.GetType());
            if (node == null) {
                node = new StateNode(state);
                nodes[state.GetType()] = node;
            }

            return node;
        }
        
        private class StateNode {
            public IState State { get; }
            public HashSet<Transition> Transitions { get; }

            public StateNode(IState state) {
                State = state;
                Transitions = new HashSet<Transition>();
            }

            public void AddTransition<T>(IState to, T predicate) {
                Transitions.Add(new Transition<T>(to, predicate));
            }
        }

        public void Dispose()
        {
            foreach (var node in nodes.Values)
            {
                node.State.Dispose();
            }
        }
    }
}