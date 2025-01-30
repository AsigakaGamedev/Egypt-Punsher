using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem attackEffect;

    public abstract void OnInit();
    public virtual void OnAttack(Vector3 attackPoint)
    {
        if (attackEffect) attackEffect.Play();
    }
}
