using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private Transform horizontalRotate;  // Объект для вращения по горизонтали
    [SerializeField] private Transform verticalRotate;    // Объект для вращения по вертикали

    [SerializeField] private float rotationSpeed = 5f; // Скорость вращения
    [SerializeField] private float minVerticalAngle = -45f; // Минимальный угол по вертикали
    [SerializeField] private float maxVerticalAngle = 45f;  // Максимальный угол по вертикали

    private bool joystickActive = false; // Активен ли джойстик

    public Animator ann;

    public PlayerAttack player;

    private void Update()
    {
        HandleRotation();

        bool isJoystickCurrentlyActive = joystick.Horizontal != 0 || joystick.Vertical != 0;

        if (joystickActive && !isJoystickCurrentlyActive)
        {
            OnJoystickReleased();
        }

        joystickActive = isJoystickCurrentlyActive;
    }

    private void HandleRotation()
    {
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        // Поворот по горизонтали (Quaternion.Lerp для плавности)
        if (horizontalInput != 0)
        {
            float targetYRotation = horizontalRotate.localEulerAngles.y + horizontalInput * rotationSpeed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.Euler(0, targetYRotation, 0);
            horizontalRotate.rotation = Quaternion.Lerp(horizontalRotate.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Поворот по вертикали в зависимости от положения джойстика
        if (verticalInput != 0 || horizontalInput != 0)
        {
            // Рассчитываем направление джойстика в 3D пространстве
            Vector3 joystickDirection = new Vector3(horizontalInput, verticalInput, 0).normalized;

        
            // Если джойстик не двигается, ничего не меняем
            if (joystickDirection.magnitude > 0)
            {

                Quaternion lookRot = Quaternion.LookRotation(new Vector3(-joystick.Horizontal, 0, -joystick.Vertical));
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);


                // Поворот в сторону направления джойстика
             //   Quaternion targetRotation = Quaternion.LookRotation(joystickDirection);

                // Ограничиваем угол наклона по вертикали
             //   float angleX = targetRotation.eulerAngles.x;
             //   if (angleX > 180) angleX -= 360; // Учитываем корректные углы
              //  angleX = Mathf.Clamp(angleX, minVerticalAngle, maxVerticalAngle);

                // Обновляем только вертикальный поворот
               // verticalRotate.rotation = Quaternion.Euler(angleX, verticalRotate.rotation.eulerAngles.y, verticalRotate.rotation.eulerAngles.z);
            }
        }
    }

    private void OnJoystickReleased()
    {
        if (ScoreManager.instance.Score > 0)
        {
            Debug.Log("Вы отпустили джойстик");
            ann.SetTrigger("throw");
            Invoke(nameof(shoooot), 1.3f);
        }
        
    }

    private void shoooot()
    {
        player.Shoot();
    }
}
