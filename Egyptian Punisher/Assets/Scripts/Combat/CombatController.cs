using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
//using Unity.Burst.CompilerServices;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] private bool hasNeededValue;
    [ShowIf(nameof(hasNeededValue)), SerializeField] private UIProgressBar progressBar;
    [ShowIf(nameof(hasNeededValue)), SerializeField] private float maxValue;
    [ShowIf(nameof(hasNeededValue)), SerializeField] private float curValue;
    [ShowIf(nameof(hasNeededValue)), SerializeField] private float valueByTime = 2;

    [Space]
    [SerializeField] private LayerMask attackAimLayer;

    [Space]
    public AttackHandler[] AllAttacks;
    public AttackHandler CurrentAttack;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;

        foreach (AttackHandler spell in AllAttacks)
        {
            spell.Init();
        }

        if (hasNeededValue)
        {
            progressBar.SetMaxValue(maxValue);
            StartCoroutine(EValueChangeByTime());
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F) && CurrentAttack)
    //    {
    //        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit, 999, attackAimLayer))
    //        {
    //            CurrentAttack.TryAttack(hit.point);
    //        }
    //    }
    //}

    public void SwitchAttack(int index)
    {
        CurrentAttack = AllAttacks[index];
    }

    public void AttackCurrent()
    {
        if (hasNeededValue && CurrentAttack.NeededValue > curValue) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 999, attackAimLayer))
        {
            if (CurrentAttack.TryAttack(hit.point))
            {
                if (hasNeededValue)
                {
                    curValue = Mathf.Clamp(curValue - CurrentAttack.NeededValue, 0, maxValue);
                    progressBar.SetValue(curValue);
                }
            }
        }
    }

    public bool AttackCurrent(Vector3 attackPoint)
    {
        if (hasNeededValue && CurrentAttack.NeededValue > curValue) return false;

        if (CurrentAttack.TryAttack(attackPoint))
        {
            if (hasNeededValue)
            {
                curValue = Mathf.Clamp(curValue - CurrentAttack.NeededValue, 0, maxValue);
                progressBar.SetValue(curValue);
            }

            return true;
        }

        return false;
    }

    private IEnumerator EValueChangeByTime()
    {
        while (hasNeededValue)
        {
            yield return new WaitForSeconds(1);
            curValue = Mathf.Clamp(curValue + valueByTime, 0, maxValue);
            progressBar.SetValue(curValue);
        }
    }
}
