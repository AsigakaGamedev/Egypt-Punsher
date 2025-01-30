using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickAttackController : MonoBehaviour
{
    [SerializeField] private Joystick attackJoystick;
    [SerializeField] private AttackHandler handler;
    [Range(0, 1), SerializeField] private float minBorder = 0.75f;

    private void Start()
    {
        handler.Init();
    }

    private void Update()
    {
        Vector3 attackDir = transform.right * attackJoystick.Horizontal + attackJoystick.Vertical * transform.forward;

        if (attackDir.sqrMagnitude >= minBorder)
        {
            handler.TryAttack(attackDir);
        }
    }
}
