using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInspectState : EnemyState
{
    [SerializeField] private EnemyNavMeshMove _enemyNavMeshMove;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private float _updateTargetRate;
    [SerializeField] private float _lostTime;

    private Coroutine _inspectingCoroutine;
    private Vector3 _target;
    
    public void ChangeTarget(Vector3 target)
    {
        if (!base.IsServer)
            return;
        _target = target;
    }
    
    public override void Enter()
    {
        if (!base.IsServer)
            return;
        _inspectingCoroutine = StartCoroutine(Inspecting());
        StartCoroutine(WaitingTimeForLost());
    }

    private IEnumerator Inspecting()
    {
        while (true)
        {
            EnemyAnimator.Move();
            if (_target != null) 
                _enemyNavMeshMove.SetDestinationServer(_target);
            yield return new WaitForSeconds(_updateTargetRate);
        }
    }
    
    private IEnumerator WaitingTimeForLost()
    {
        yield return new WaitForSeconds(_lostTime);
        StopCoroutine(_inspectingCoroutine);
        _enemyStateMachine.Patrol();
    }

    public override void Exit()
    {
        if (!base.IsServer)
            return;
        StopAllCoroutines();
        base.Exit();
    }
}
