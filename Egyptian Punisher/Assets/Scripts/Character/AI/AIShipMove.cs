using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShipMove : AIBehvaiour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float enemyCheckDst;

    [Space]
    [ReadOnly, SerializeField] private Collider enemy;

    public override bool OnCheckEnter()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyCheckDst, enemyLayer);

        if (colliders.Length > 0)
        {
            enemy = colliders[0];
            return true;
        }

        return false;
    }

    public override void OnUpdateLogic()
    {
        Vector3 dir = enemy.transform.position - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 1);
        //character.MoveTo(dir, pursueSpeed);
    }
}
