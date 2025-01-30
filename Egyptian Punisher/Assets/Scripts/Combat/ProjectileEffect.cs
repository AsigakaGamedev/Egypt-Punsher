using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEffect : AttackEffect
{
    [SerializeField] private ProjectileObject prefab;
    [SerializeField] private float throwForce = 200;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform targetPoint;

    private ObjectPoolingManager poolingManager;

    public override void OnInit()
    {
        poolingManager = ServiceLocator.GetService<ObjectPoolingManager>();
    }

    public override void OnAttack(Vector3 attackPoint)
    {
        base.OnAttack(attackPoint);

        ProjectileObject newProjectile = poolingManager.GetPoolable(prefab);
        newProjectile.transform.position = startPoint.position;
        Vector3 throwDir = targetPoint.position - startPoint.position;
        throwDir.Normalize();
        newProjectile.ThrowTo(throwDir, throwForce);
    }
}
