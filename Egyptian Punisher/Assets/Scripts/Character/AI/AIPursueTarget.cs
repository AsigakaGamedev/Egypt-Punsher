using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPursueTarget : AIBehvaiour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float enemyCheckDst;

    [Space]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed = 0.5f;
    [SerializeField] private Vector3 rotateUpwords = Vector3.forward;
    [SerializeField] private Vector3 offsetRotation;

    [Space]
    [SerializeField] private AttackHandler attackHandler;
    [SerializeField] private float attackDistance;

    [Space]
    [SerializeField] private bool showDebug = true;

    [Space]
    [ReadOnly, SerializeField] private Collider enemy;
    [SerializeField] private NavMeshAgent agent;

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyCheckDst);
        }
    }

    public override void OnInit(Character character)
    {
        base.OnInit(character);

        if (attackHandler) attackHandler.Init();
    }

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

    public override void OnEnterLogic()
    {
        agent.stoppingDistance = attackDistance;
        agent.speed = moveSpeed;
    }

    public override void OnUpdateLogic()
    {
        agent.SetDestination(enemy.transform.position);

        if (Vector3.Distance(transform.position, enemy.transform.position) <= attackDistance)
        {
            agent.isStopped = true;
            attackHandler.TryAttack(enemy.transform.position);
            //Vector3 dir = enemy.transform.position - transform.position;
            //transform.rotation = Quaternion.LookRotation(dir);
        }
        else
        {
            agent.isStopped = false;
        }
    }
}