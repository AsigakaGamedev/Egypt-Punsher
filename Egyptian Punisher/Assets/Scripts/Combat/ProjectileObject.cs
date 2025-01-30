using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileObject : PoolableObject
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float damage = 5;
    [Tag, SerializeField] private List<string> damagableTags;
    [Tag, SerializeField] private List<string> hideTags;

    private void OnDisable()
    {
        rb.linearVelocity = Vector3.zero;
    }

    private void OnValidate()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (damagableTags.Contains(other.gameObject.tag))
        {
            if (other.TryGetComponent(out HealthComponent health))
            {
                health.Damage(damage);
            }
            else
            {
                HealthComponent enemyHealth = other.GetComponentInParent<HealthComponent>();
                enemyHealth.Damage(damage);
            }

            gameObject.SetActive(false);
        }
        else if (hideTags.Contains(other.gameObject.tag))
        {
            gameObject.SetActive(false);
        }
    }

    public void ThrowTo(Vector3 target, float force)
    {
        Vector3 direction = target - transform.position;
        direction.Normalize();

        transform.rotation = Quaternion.LookRotation(target);

        rb.AddForce(target * force, ForceMode.Impulse);
    }
}
