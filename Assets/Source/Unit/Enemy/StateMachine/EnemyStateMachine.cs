using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    [SerializeField] private EnemyState _attack;
    [SerializeField] private EnemyState _patrol;
    [SerializeField] private EnemyChaseState _chase;
    [SerializeField] private EnemyInspectState _inspect;

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