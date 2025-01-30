using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private HealthComponent health;

    private void OnEnable()
    {
        GetComponent<HealthComponent>().enabled = true;
    }

    public void Init()
    {
        health = GetComponent<HealthComponent>();
    }

    public void MoveTo(Vector3 dir, float moveSpeed)
    {
        dir.Normalize();
        rb.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
    }

    public void RotateTo(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
