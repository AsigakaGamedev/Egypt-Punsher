using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdle : AIBehvaiour
{
    [SerializeField] private Animator animator;

    public override void OnEnterLogic()
    {
        base.OnEnterLogic();

        animator.SetBool("Idle", true);
    }

    public override void OnExitLogic()
    {
        base.OnExitLogic();

        animator.SetBool("Idle", false);
    }
}
