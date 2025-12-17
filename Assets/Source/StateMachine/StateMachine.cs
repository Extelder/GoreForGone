using System;
using FishNet.Object;
using UnityEngine;

public class StateMachine : NetworkBehaviour
{
    [SerializeField] private bool _notStartOnEnable;
    [SerializeField] protected State _startState;
    public State CurrentState { get; protected set; }

    public event Action<State> StatePrepareToChange;

    public void DefaultState()
    {
        ChangeState(_startState);
    }

    public void Init()
    {
        if (_notStartOnEnable)
        {
            CurrentState = _startState;

            return;
        }

        CurrentState = _startState;
        CurrentState.Enter();
    }

    public override void OnStartClient()
    {
        Init();
    }


    public void ChangeState(State state)
    {
        if (CurrentState.CanChanged && CurrentState != state)
        {
            StatePrepareToChange?.Invoke(state);
            CurrentState.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }
    }
}