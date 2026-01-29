using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnarableEnemyRagdollHitBox : EnemyRagdollHitBox
{
    [SerializeField] private EnemyVulnarableChecker _enemyVulnarableChecker;

    public override void Visit(SwordRaycastAttack swordRaycastAttack, Vector3 hitPoint)
    {
        _enemyVulnarableChecker.PerformCheck();

        base.Visit(swordRaycastAttack, hitPoint);
    }
}