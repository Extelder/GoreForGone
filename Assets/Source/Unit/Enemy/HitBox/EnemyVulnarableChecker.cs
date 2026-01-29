using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVulnarableChecker : MonoBehaviour
{
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private EnemyState[] _nonVulnarableStates;

    [SerializeField] private EnemyHealth _health;

    public void PerformCheck()
    {
        if (IsEnemyVulnarable())
        {
            _health.TakeDamage(_health.MaxValue);
        }
    }

    public bool IsEnemyVulnarable()
    {
        for (int i = 0; i < _nonVulnarableStates.Length; i++)
        {
            if (_nonVulnarableStates[i] == _enemyStateMachine.CurrentState)
                return false;
        }

        return true;
    }
}