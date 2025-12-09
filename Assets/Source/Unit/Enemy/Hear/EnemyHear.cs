using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHear : MonoBehaviour
{
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    
    public void SoundHeared(Transform target)
    {
        _enemyStateMachine.Inspect(target.position);
    }
}
