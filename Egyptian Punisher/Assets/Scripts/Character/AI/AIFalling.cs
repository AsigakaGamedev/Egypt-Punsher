using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFalling : AIBehvaiour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 2;

    [Space]
    [SerializeField] private Animator animator;
    [SerializeField, AnimatorParam(nameof(animator))] private string animFallingKey = "isFalling";

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * groundCheckDistance);
    }

    public override bool OnCheckEnter()
    {
        return !Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    public override void OnEnterLogic()
    {
        animator.SetBool(animFallingKey, true);
    }

    public override void OnExitLogic()
    {
        animator.SetBool(animFallingKey, false);
    }
}
