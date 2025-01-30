using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAttackTrigger : MonoBehaviour
{
    [SerializeField] private AttackHandler[] attacks;

    [Space]
    [SerializeField] private LayerMask checkLayer;
    [SerializeField] private float checkDistance;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * checkDistance);
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, checkDistance, checkLayer))
        {
            foreach (var att in attacks)
            {
                att.TryAttack();
            }
        }
    }
}
