using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    [SerializeField] private EnemyState _attack;
    [SerializeField] private EnemyState _patrol;
    [SerializeField] private EnemyChaseState _chase;
    
    public void Chase(Transform targetPosition)
    {
        _chase.ChangeTarget(targetPosition);
        ChangeState(_chase);
    }

    public void Attack()
    {
        ChangeState(_attack);
    }

    public void Patrol()
    {
        ChangeState(_patrol);
    }
}