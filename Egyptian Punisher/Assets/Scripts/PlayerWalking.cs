using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalking : MonoBehaviour
{
    [SerializeField] private Joystick moveJoystick;  // �������� ��� ��������
    [SerializeField] private Animator animator;      // �������� ��� ���������� ���������
    [SerializeField] private float moveSpeed = 5f;   // �������� ��������
    [SerializeField] private float rotationSpeed = 10f;  // �������� ��������
    [SerializeField] private GameObject Arrow;       // ������ ��� ���������
    [SerializeField] private GameObject Arro2w;      // �������������� ������ ������
    private Rigidbody rb;                            // ��������� Rigidbody ��� ������
    private Vector3 movement;                       // ������ ��������

    private void Start()
    {
        // �������� ��������� Rigidbody ��� ������������� � ��������
        rb = GetComponent<Rigidbody>();

        // ��������� �������� �� PlayerPrefs (���� ����)
        moveSpeed += PlayerPrefs.GetInt("speed", 0);
    }

    private void Update()
    {
        // ��������� ������ (���� ��� �����)
        Arrow.SetActive(false);
        Arro2w.SetActive(false);

        // �������� ����������� �������� �� ���������
        movement = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical);

        // ��������� ��������, �������� �������� �� ��� Y (�� ���������) ��� ����������
        rb.linearVelocity = movement * moveSpeed + new Vector3(0, rb.linearVelocity.y, 0);

        // ���������, ���� �������� �������� (���������� sqrMagnitude ��� ����� ������� ��������)
        if (movement.sqrMagnitude > 0.1f) // ���������� ��������� ��������, ����� �������� ������������ �������� �� ����� ��������
        {
            // ���� �������� ���������, �������� �������� "walk"
            if (animator) animator.SetBool("walk", true);

            // ������������ ���� ��������, ����� �������� �������� � ����������� ��������
            Quaternion lookRot = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
        }
        else
        {
            // ���� �������� �� ���������, ��������� �������� "walk"
            if (animator) animator.SetBool("walk", false);
        }
    }
}
