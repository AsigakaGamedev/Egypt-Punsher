using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slign : MonoBehaviour
{
    [SerializeField] private Transform jumpPoint;
    [SerializeField] private float pullSpeed = 2f; // �������� ������������
    [SerializeField] private float stopDistance = 0.1f; // ���������, ��� ������� �������, ��� ����� ������ �����

    private Coroutine pullCoroutine; // ������ �� ������� �������� ������������
    private bool isInTrigger = false; // ����, �����������, ��������� �� ����� � ��������
    private Rigidbody rb; // ������ �� Rigidbody ������
    private PlayerMovement playerMovementScript; // ������ �� ������ PlayerMovement
    private Joystick moveJoystick; // ������ �� ��������� ���������

    private void Start()
    {
        // �������� ������ �� ����������
        playerMovementScript = FindObjectOfType<PlayerMovement>();
        moveJoystick = FindObjectOfType<Joystick>(); // ������������, ��� �������� ��������� �� �����
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerMovement>().moveJoystick.gameObject.SetActive(false);
        other.GetComponent<PlayerMovement>().moveJoystick.gameObject.SetActive(true);


        if (other.CompareTag("Player"))
        {
            // �������� ���������� ������
            rb = other.GetComponent<Rigidbody>();
            PlayerMovement jumpp = other.GetComponent<PlayerMovement>();

            if (rb != null)
            {
                rb.useGravity = false; // ��������� ����������
               rb.linearVelocity = Vector3.zero; // �������� ��������, ����� �������� �������� ��������
            }

            jumpp.enabled = true;
            other.GetComponent<PlayerWalking>().enabled = false;

            jumpp.SetKnee();

            // ��������� �������� ������������ � jumpPoint
            isInTrigger = true;
            pullCoroutine = StartCoroutine(PullToJumpPoint(other.transform));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (rb != null)
            {
                rb.useGravity = true; // �������� ����������
                print("Vernul  gravity");
                
            }

            // ������������� ������������, ���� ��� ���� ��������
            if (pullCoroutine != null)
            {
                StopCoroutine(pullCoroutine);
                pullCoroutine = null;
            }

            // ��������� PlayerMovement � �������� PlayerWalking
            other.GetComponent<PlayerMovement>().enabled = false;
            other.GetComponent<PlayerWalking>().enabled = true;

            // ������������� �������� ������ � ������ ������ �� ��������
            rb.linearVelocity = Vector3.zero; // �������� ��������
            isInTrigger = false;
        }
    }

    private IEnumerator PullToJumpPoint(Transform player)
    {
        // ����������� ������ � ����� jumpPoint
        while (isInTrigger && Vector3.Distance(player.position, jumpPoint.position) > stopDistance) // ���� ����� �� �������� �� jumpPoint
        {
            // ������ ���������� ������ � ������� jumpPoint, ������� ��� Y
            Vector3 targetPosition = new Vector3(jumpPoint.position.x, jumpPoint.position.y, jumpPoint.position.z);
            player.position = Vector3.MoveTowards(player.position, targetPosition, pullSpeed * Time.deltaTime);

            yield return null; // ��� ��������� ����
        }

        // ����� ������ ����� ������
        if (isInTrigger && Vector3.Distance(player.position, jumpPoint.position) <= stopDistance)
        {
            Debug.Log("Player reached jumpPoint!");

            // ������, ����� ����� ������ �����, �������� ����������� ��������� ���������
            while (isInTrigger)
            {
                print("�� �����");
                playerMovementScript.touch = false;

                // ���� �������� �������, ���������� ������������
                if (moveJoystick != null && Mathf.Abs(moveJoystick.Horizontal) < 0.1f && Mathf.Abs(moveJoystick.Vertical) < 0.1f)
                {
                    playerMovementScript.GetComponent<Rigidbody>().useGravity = true;
                    print("������");
                    print("Joystick ������� ����� ���������� �����");
                    StopCoroutine(pullCoroutine);
                    pullCoroutine = null;
                    Destroy(gameObject);
                    print("����������� ������");
                   // playerMovementScript.GetComponent<PlayerWalking>().enabled = true;
                  //  playerMovementScript.enabled = false;
                    break;
                }

                yield return null; // ��� ��������� ����
            }
        }
    }
}
