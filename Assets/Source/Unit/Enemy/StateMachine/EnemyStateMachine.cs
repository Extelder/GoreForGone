using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    [SerializeField] private bool _canReact;

    [ShowIf(nameof(_canReact))] [SerializeField]
    private EnemyState _react;
    [SerializeField] private EnemyState _attack;
    [SerializeField] private EnemyState _patrol;
    [SerializeField] private EnemyChaseState _chase;
    [SerializeField] private EnemyInspectState _inspect;

    public void React()
    {
        if (!base.IsServer)
            return;
        if (!_canReact)
            return;
        CurrentState.CanChanged = true;
        ChangeState(_react);
    }
    
    public void Inspect(Vector3 targetPosition)
    {
        if (!base.IsServer)
            return;
        _inspect.ChangeTarget(targetPosition);
        ChangeState(_inspect);
    }
    
    public void Attack()
    {
        if (!base.IsServer)
            return;
        if (CurrentState == _attack)
            return;
        CurrentState.CanChanged = true;
        ChangeState(_attack);
    }

    public void Patrol()
    {
        if (!base.IsServer)
            return;
        ChangeState(_patrol);
    }

    public void Chase(Transform targetPosition)
    {
        if (!base.IsServer)
            return;
        _chase.ChangeTarget(targetPosition);
        ChangeState(_chase);
    }
}