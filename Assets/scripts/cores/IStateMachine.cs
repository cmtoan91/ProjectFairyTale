using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class IStateMachine<T, U> : MonoBehaviour where T : struct, Enum
                                                            where U : struct, Enum
{
    Dictionary<T, IState> _states = new Dictionary<T, IState>();
    [SerializeField]
    T _currentStateType;
    public T CurrentStateType { get { return _currentStateType; } }
    public abstract T UnassignedType { get; }
    protected IState CurrentState
    {
        get
        {
            IState currentState = null;
            if (_states.TryGetValue(_currentStateType, out currentState))
                return currentState;
            else
                return null;
        }
    }
    //register state
    public void RegisterState(IState state)
    {
        _states[state.Type] = state;
    }
    bool _changingState = false;
    //change state
    public virtual bool ChangeState(T stateType, params object[] args)
    {
        if (stateType.CompareTo(UnassignedType) == 0)
        {
            return false;
        }

        if (_changingState)
        {
            return false;
        }

        if (_currentStateType.CompareTo(stateType) == 0)
            return false;

        IState newState;
        if (!_states.TryGetValue(stateType, out newState))
            return false;

        _changingState = true;

        IState currentState = CurrentState;
        if (currentState != null)
            currentState.OnStateExit(stateType, args);

        newState.OnStateEnter(_currentStateType, args);
        _currentStateType = stateType;

        _changingState = false;

        return true;
    }
    //update brain
    public virtual void UpdateMachine()
    {
        var current = CurrentState;
        if (current != null)
            current.OnStateUpdate();
    }

    public bool HasState(T type)
    {
        return _states.ContainsKey(type);
    }
    public void SendMessageToBrain(U msgtype, params object[] args)
    {
        CurrentState.OnReceiveMessage(msgtype, args);
    }

    public abstract class IState
    {
        public IStateMachine<T, U> Machine;
        public IState(IStateMachine<T, U> machine)
        {
            Machine = machine;
        }
        public abstract T Type { get; }
        public virtual void OnStateEnter(T prevStateType, object[] args) { }
        public virtual void OnStateUpdate() { }
        public virtual void OnStateExit(T newStateType, object[] arg) { }
        public virtual void OnReceiveMessage(U msgtype, object[] args) { }
    }
}
