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
    [SerializeField] private float maxJumpDistance = 10f; // Максимальная дальность прыжка
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject landingPlace;

    [Space]
    [SerializeField] private LayerMask islandLayerMask;
    [SerializeField] private float raycastDistance;

    private Rigidbody rb;
    private bool isChargingJump = false;
    private bool isGrounded = false;
    private bool wasJoystickActive = false; // Состояние предыдущего кадра
    private bool hasJumpTriggered = false; // Флаг, чтобы сработал триггер jump только один раз
    private float jumpPower = 0f;
    private float maxJumpPower = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (landingPlace) landingPlace.SetActive(false); // Скрываем индикатор места приземления в начале
    }

    //public bool CanJump;

    private void Update()
    {
        if (isWalk) return;

       //if (touch) return;
        // Проверка, находится ли игрок на земле
        isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance, islandLayerMask);

        Vector3 moveDirection = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical);

        // Проверяем, активен ли джойстик
        bool isJoystickActive = moveDirection.magnitude > 0.1f;

        // Если джойстик только что начали использовать (первое касание)
        if (isJoystickActive && !wasJoystickActive)
        {
            print("TAAAAAAAAAAAAAAAAK JOYSTCIJ");
            // Вызываем триггер "knee" один раз
           // if (animator) animator.ResetTrigger("jump");
           // if (animator) animator.ResetTrigger("IdleTr");
            if (animator) animator.SetTrigger("knee");
        }

        // Если джойстик был отпущен и триггер прыжка ещё не сработал
        if (!isJoystickActive && wasJoystickActive && isGrounded && !hasJumpTriggered)
        {
            // Вызываем триггер "jump" один раз при отпускании джойстика
            if (animator) animator.SetTrigger("jump");
            hasJumpTriggered = true; // Устанавливаем флаг, чтобы триггер не сработал снова
        }

        // Сохраняем текущее состояние джойстика
        wasJoystickActive = isJoystickActive;

        // Обновление анимации движения
      //  if (animator) animator.SetBool("idle", isJoystickActive);
       // if (animator) animator.SetTrigger("IdleTr");

        if (moveDirection != Vector3.zero)
        {
            // Рассчитываем расстояние от центра джойстика
            float distanceFromCenter = new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical).magnitude;
            float normalizedDistance = Mathf.Clamp01(distanceFromCenter / maxDistanceFromCenter);

            // Если джойстик натянут и мы не начали зарядку прыжка
            if (!isChargingJump && isGrounded)
            {
                isChargingJump = true;
                StartCoroutine(ChargeJump(normalizedDistance));
            }

            // Поворот игрока в сторону движения джойстика
            Quaternion lookRot = Quaternion.LookRotation(new Vector3(-moveJoystick.Horizontal, 0, -moveJoystick.Vertical));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);

            // Обновляем положение индикатора приземления
            UpdateLandingPlace(moveDirection);
        }
        else
        {
            // Если джойстик отпущен и зарядка прыжка была завершена
            if (isChargingJump)
            {
                isChargingJump = false;

                if (isGrounded) // Прыжок возможен только если игрок на земле
                {
                    // CanJump = true;
                    if (animator) animator.ResetTrigger("knee");
                    if (animator) animator.ResetTrigger("IdleTr");
                    if (animator) animator.SetTrigger("Jump");



                    Jump(); // Выполняем прыжок
                }
            }

            // Скрываем индикатор приземления, если джойстик не тянут
            if (landingPlace) landingPlace.SetActive(false);
        }
    }

    private IEnumerator ChargeJump(float normalizedDistance)
    {
        // Зарядка прыжка
        jumpPower = 0f; // Сбрасываем силу прыжка перед началом зарядки

        // Заряжаем прыжок в зависимости от того, как сильно натянут джойстик
        while (isChargingJump && jumpPower < maxJumpPower)
        {
            jumpPower += Time.deltaTime * maxJumpPower * normalizedDistance;
            jumpPower = Mathf.Clamp(jumpPower, 0, maxJumpPower);
            UpdateLandingPlace(transform.forward); // Постоянно обновляем позицию индикатора
            yield return null;
        }
    }

    private void Jump()
    {
        // Направление прыжка
        Vector3 jumpDirection = transform.forward;

        // Ограничиваем дальность прыжка
        float clampedJumpPower = Mathf.Clamp(jumpPower, 0, maxJumpDistance);

        // Используем текущую силу прыжка
        Vector3 force = jumpDirection * clampedJumpPower * jumpForceMultiplier + Vector3.up * clampedJumpPower * jumpForceMultiplier;
        rb.AddForce(force, ForceMode.Impulse);

        // Сброс силы прыжка после прыжка
        jumpPower = 0f;

        // Начинаем отслеживать приземление
        StartCoroutine(WaitForLanding());
    }

    private IEnumerator WaitForLanding()
    {
       // CanJump = false;
        // Ждем, пока игрок снова окажется на земле
        while (!isGrounded)
        {
            yield return null;
        }

        // Приземление произошло, сбрасываем триггер
        // if (animator) animator.ResetTrigger("jump");
        //if (animator) animator.SetBool("idle", true);
    }

    private void UpdateLandingPlace(Vector3 moveDirection)
    {
        if (!landingPlace) return;

        // Активируем индикатор места приземления
        landingPlace.SetActive(true);

        // Направление прыжка
        Vector3 jumpDirection = transform.forward.normalized;

        // Прогнозируемое место приземления на основе силы прыжка и направления
        Vector3 predictedPosition = transform.position + jumpDirection * Mathf.Clamp(jumpPower * jumpForceMultiplier, 0, maxJumpDistance);

        // Обновляем только координаты по осям X и Z, оставляя Y неизменным
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
            // Игрок коснулся земли
            //obj.enabled = true;
            print("На землю");
            fallEffect.Play();

            if (animator) animator.ResetTrigger("knee");
            if (animator) animator.ResetTrigger("jump");
            if (animator) animator.SetBool("walk", false);

            if (animator) animator.SetTrigger("IdleTr");
           
            // Сбрасываем флаг, чтобы прыжок мог сработать снова
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

        // Сбрасываем флаг, чтобы прыжок мог сработать снова
        hasJumpTriggered = false;

        Vector3 moveDirection = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical);

        bool isJoystickActive = moveDirection.magnitude > 0.1f;


        if (isJoystickActive)
        {
            print("TAAAAAAAAAAAAAAAAK JOYSTCIJ");
            // Вызываем триггер "knee" один раз
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
    //[SerializeField] private Joystick moveJoystick;  // Джойстик для движения
 //   [SerializeField] private Animator animator;      // Аниматор для управления анимацией
    //[SerializeField] private float moveSpeed = 5f;   // Скорость движения
    [SerializeField] private float rotationSpeed2 = 10f;  // Скорость вращения
    [SerializeField] private GameObject Arrow;       // Стрела для ориентира
    [SerializeField] private GameObject Arro2w;      // Дополнительный объект стрелы
    //private Rigidbody rb;                            // Компонент Rigidbody для физики
    private Vector3 movement;                       // Вектор движения

    //  private void Start()
    // {
    // Получаем компонент Rigidbody для использования в движении
    //  rb = GetComponent<Rigidbody>();

    // Загружаем скорость из PlayerPrefs (если есть)
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
        // Отключаем стрелы (если это нужно)
        landingPlace.SetActive(false);
        //Arro2w.SetActive(false);

        // Получаем направление движения от джойстика
        movement = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical);

        // Применяем движение, оставляя скорость по оси Y (по вертикали) для гравитации
        rb.velocity = movement * moveSpeed + new Vector3(0, rb.velocity.y, 0);

        // Проверяем, если персонаж движется (используем sqrMagnitude для более быстрой проверки)
        if (movement.sqrMagnitude > 0.1f) // Используем пороговое значение, чтобы избежать срабатывания анимации на малые движения
        {
            // Если персонаж двигается, включаем анимацию "walk"
            if (animator) animator.SetBool("walk", true);

            // Рассчитываем угол поворота, чтобы персонаж вращался в направлении движения
            Quaternion lookRot = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed2);
        }
        else
        {
            // Если персонаж не двигается, отключаем анимацию "walk"
            if (animator) animator.SetBool("walk", false);
        }
    }

}