using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAttackDroper : UIMovableObject
{
    [SerializeField] private int attackIndex;
    [SerializeField] private CombatController combatController;

    protected override void OnEnd()
    {
        combatController.SwitchAttack(attackIndex);
        combatController.AttackCurrent();
    }
}
