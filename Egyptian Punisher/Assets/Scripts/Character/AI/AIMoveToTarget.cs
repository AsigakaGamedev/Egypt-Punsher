using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveToTarget : AIBehvaiour
{
    [SerializeField] private int targetID;
    [SerializeField] private float targetMoveRadius;

    [Space]
    [SerializeField] private float moveSpeed;

    private AIMoveTargetsHandler targetsHandler;
    private Vector3 targetPos;

    public override void OnInit(Character character)
    {
        base.OnInit(character);

        targetsHandler = ServiceLocator.GetService<AIMoveTargetsHandler>();
        targetPos = targetsHandler.Containers[targetID].Targets[Random.Range(0, targetsHandler.Containers[targetID].Targets.Length)].position + 
                               new Vector3(Random.Range(- targetMoveRadius, targetMoveRadius), 0, Random.Range(-targetMoveRadius, targetMoveRadius));
    }

    public override void OnUpdateLogic()
    {
        Vector3 moveDir = targetPos - transform.position;
        character.MoveTo(moveDir, moveSpeed);
        character.RotateTo(targetPos);
    }
}
