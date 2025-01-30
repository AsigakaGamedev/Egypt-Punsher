using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private ParticleSystem fallEffect;

    public Joystick moveJoystick;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpForceMultiplier;
    [SerializeField] private float maxDistanceFromCenter;
    [SerializeField] private float maxJumpDistance = 10f; // ������������ ��������� ������
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject landingPlace;

    [Space]
    [SerializeField] private LayerMask islandLayerMask;
    [SerializeField] private float raycastDistance;

    private Rigidbody rb;
    private bool isChargingJump = false;
    private bool isGrounded = false;
    private bool wasJoystickActive = false; // ��������� ����������� �����
    private bool hasJumpTriggered = false; // ����, ����� �������� ������� jump ������ ���� ���
    private float jumpPower = 0f;
    private float maxJumpPower = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (landingPlace) landingPlace.SetActive(false); // �������� ��������� ����� ����������� � ������
    }

    //public bool CanJump;

    private void Update()
    {
        if (isWalk) return;

       //if (touch) return;
        // ��������, ��������� �� ����� �� �����
        isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance, islandLayerMask);

        Vector3 moveDirection = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical);

        // ���������, ������� �� ��������
        bool isJoystickActive = moveDirection.magnitude > 0.1f;

        // ���� �������� ������ ��� ������ ������������ (������ �������)
        if (isJoystickActive && !wasJoystickActive)
        {
            print("TAAAAAAAAAAAAAAAAK JOYSTCIJ");
            // �������� ������� "knee" ���� ���
           // if (animator) animator.ResetTrigger("jump");
           // if (animator) animator.ResetTrigger("IdleTr");
            if (animator) animator.SetTrigger("knee");
        }

        // ���� �������� ��� ������� � ������� ������ ��� �� ��������
        if (!isJoystickActive && wasJoystickActive && isGrounded && !hasJumpTriggered)
        {
            // �������� ������� "jump" ���� ��� ��� ���������� ���������
            if (animator) animator.SetTrigger("jump");
            hasJumpTriggered = true; // ������������� ����, ����� ������� �� �������� �����
        }

        // ��������� ������� ��������� ���������
        wasJoystickActive = isJoystickActive;

        // ���������� �������� ��������
      //  if (animator) animator.SetBool("idle", isJoystickActive);
       // if (animator) animator.SetTrigger("IdleTr");

        if (moveDirection != Vector3.zero)
        {
            // ������������ ���������� �� ������ ���������
            float distanceFromCenter = new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical).magnitude;
            float normalizedDistance = Mathf.Clamp01(distanceFromCenter / maxDistanceFromCenter);

            // ���� �������� ������� � �� �� ������ ������� ������
            if (!isChargingJump && isGrounded)
            {
                isChargingJump = true;
                StartCoroutine(ChargeJump(normalizedDistance));
            }

            // ������� ������ � ������� �������� ���������
            Quaternion lookRot = Quaternion.LookRotation(new Vector3(-moveJoystick.Horizontal, 0, -moveJoystick.Vertical));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);

            // ��������� ��������� ���������� �����������
            UpdateLandingPlace(moveDirection);
        }
        else
        {
            // ���� �������� ������� � ������� ������ ���� ���������
            if (isChargingJump)
            {
                isChargingJump = false;

                if (isGrounded) // ������ �������� ������ ���� ����� �� �����
                {
                    // CanJump = true;
                    if (animator) animator.ResetTrigger("knee");
                    if (animator) animator.ResetTrigger("IdleTr");
                    if (animator) animator.SetTrigger("Jump");



                    Jump(); // ��������� ������
                }
            }

            // �������� ��������� �����������, ���� �������� �� �����
            if (landingPlace) landingPlace.SetActive(false);
        }
    }

    private IEnumerator ChargeJump(float normalizedDistance)
    {
        // ������� ������
        jumpPower = 0f; // ���������� ���� ������ ����� ������� �������

        // �������� ������ � ����������� �� ����, ��� ������ ������� ��������
        while (isChargingJump && jumpPower < maxJumpPower)
        {
            jumpPower += Time.deltaTime * maxJumpPower * normalizedDistance;
            jumpPower = Mathf.Clamp(jumpPower, 0, maxJumpPower);
            UpdateLandingPlace(transform.forward); // ��������� ��������� ������� ����������
            yield return null;
        }
    }

    private void Jump()
    {
        // ����������� ������
        Vector3 jumpDirection = transform.forward;

        // ������������ ��������� ������
        float clampedJumpPower = Mathf.Clamp(jumpPower, 0, maxJumpDistance);

        // ���������� ������� ���� ������
        Vector3 force = jumpDirection * clampedJumpPower * jumpForceMultiplier + Vector3.up * clampedJumpPower * jumpForceMultiplier;
        rb.AddForce(force, ForceMode.Impulse);

        // ����� ���� ������ ����� ������
        jumpPower = 0f;

        // �������� ����������� �����������
        StartCoroutine(WaitForLanding());
    }

    private IEnumerator WaitForLanding()
    {
       // CanJump = false;
        // ����, ���� ����� ����� �������� �� �����
        while (!isGrounded)
        {
            yield return null;
        }

        // ����������� ���������, ���������� �������
        // if (animator) animator.ResetTrigger("jump");
        //if (animator) animator.SetBool("idle", true);
    }

    private void UpdateLandingPlace(Vector3 moveDirection)
    {
        if (!landingPlace) return;

        // ���������� ��������� ����� �����������
        landingPlace.SetActive(true);

        // ����������� ������
        Vector3 jumpDirection = transform.forward.normalized;

        // �������������� ����� ����������� �� ������ ���� ������ � �����������
        Vector3 predictedPosition = transform.position + jumpDirection * Mathf.Clamp(jumpPower * jumpForceMultiplier, 0, maxJumpDistance);

        // ��������� ������ ���������� �� ���� X � Z, �������� Y ����������
        landingPlace.transform.position = new Vector3(predictedPosition.x, landingPlace.transform.position.y, predictedPosition.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastDistance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isWalk) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Island"))
        {
            // ����� �������� �����
            //obj.enabled = true;
            print("�� �����");
            fallEffect.Play();

            if (animator) animator.ResetTrigger("knee");
            if (animator) animator.ResetTrigger("jump");
            if (animator) animator.SetBool("walk", false);

            if (animator) animator.SetTrigger("IdleTr");
           
            // ���������� ����, ����� ������ ��� ��������� �����
            hasJumpTriggered = false;
            return;
        }
    }

    public void SetKnee()
    {
        if (animator) animator.ResetTrigger("knee");
        if (animator) animator.ResetTrigger("jump");

        if (animator) animator.SetTrigger("IdleTr");
        // print("SetWWalk");
       moveJoystick.gameObject.SetActive(false);
        moveJoystick.gameObject.SetActive(true);
        // if (animator) animator.ResetTrigger("knee");
        // if (animator) animator.SetBool("walk", true);
        // if (animator) animator.SetBool("walk", false);

        if (animator) animator.ResetTrigger("knee");
        if (animator) animator.ResetTrigger("jump");
        if (animator) animator.SetBool("walk", false);

        if (animator) animator.SetTrigger("IdleTr");

        // ���������� ����, ����� ������ ��� ��������� �����
        hasJumpTriggered = false;

        Vector3 moveDirection = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical);

        bool isJoystickActive = moveDirection.magnitude > 0.1f;


        if (isJoystickActive)
        {
            print("TAAAAAAAAAAAAAAAAK JOYSTCIJ");
            // �������� ������� "knee" ���� ���
            // if (animator) animator.ResetTrigger("jump");
            // if (animator) animator.ResetTrigger("IdleTr");
            if (animator) animator.SetTrigger("knee");
        }
        //  wasJoystickActive = true;
    }

    public void SetIdle()
    {
        print("SetWWalk");
        moveJoystick.gameObject.SetActive(false);
        moveJoystick.gameObject.SetActive(true);
        if (animator) animator.ResetTrigger("knee");
      //  if (animator) animator.ResetTrigger("IdleTr");
        if (animator) animator.ResetTrigger("jump");
        if (animator) animator.SetBool("walk", false);

        if (animator) animator.SetTrigger("IdleTr");

        // if (animator) animator.SetBool("walk", true);
  
    }




    //public bool touch = true;
   // public PlayerWalking obj;



    [Space]
    //[SerializeField] private Joystick moveJoystick;  // �������� ��� ��������
 //   [SerializeField] private Animator animator;      // �������� ��� ���������� ���������
    //[SerializeField] private float moveSpeed = 5f;   // �������� ��������
    [SerializeField] private float rotationSpeed2 = 10f;  // �������� ��������
    [SerializeField] private GameObject Arrow;       // ������ ��� ���������
    [SerializeField] private GameObject Arro2w;      // �������������� ������ ������
    //private Rigidbody rb;                            // ��������� Rigidbody ��� ������
    private Vector3 movement;                       // ������ ��������

    //  private void Start()
    // {
    // �������� ��������� Rigidbody ��� ������������� � ��������
    //  rb = GetComponent<Rigidbody>();

    // ��������� �������� �� PlayerPrefs (���� ����)
    //  moveSpeed += PlayerPrefs.GetInt("speed", 0);
    //  }


    public bool isWalk;

    private void Awake()
    {
        moveSpeed += PlayerPrefs.GetInt("speed", 0);
    }

    private void FixedUpdate()
    {
        if (!isWalk) return;
        // ��������� ������ (���� ��� �����)
        landingPlace.SetActive(false);
        //Arro2w.SetActive(false);

        // �������� ����������� �������� �� ���������
        movement = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical);

        // ��������� ��������, �������� �������� �� ��� Y (�� ���������) ��� ����������
        rb.velocity = movement * moveSpeed + new Vector3(0, rb.velocity.y, 0);

        // ���������, ���� �������� �������� (���������� sqrMagnitude ��� ����� ������� ��������)
        if (movement.sqrMagnitude > 0.1f) // ���������� ��������� ��������, ����� �������� ������������ �������� �� ����� ��������
        {
            // ���� �������� ���������, �������� �������� "walk"
            if (animator) animator.SetBool("walk", true);

            // ������������ ���� ��������, ����� �������� �������� � ����������� ��������
            Quaternion lookRot = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed2);
        }
        else
        {
            // ���� �������� �� ���������, ��������� �������� "walk"
            if (animator) animator.SetBool("walk", false);
        }
    }

}