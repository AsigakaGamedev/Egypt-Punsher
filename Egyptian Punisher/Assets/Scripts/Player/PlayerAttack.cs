using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform shootingPoint; // ����� ��������
    [SerializeField] private GameObject projectilePrefab; // ������ �������
    [SerializeField] private Button shootButton; // ������ ��� ��������
    [SerializeField] private Transform trajectoryMarker; // ������ ��� ������ ����� �������
    [SerializeField] private float shootingForce = 10f; // ���� ��������
    [SerializeField] private float gravity = -9.8f; // ����������

    [SerializeField] private ParticleSystem effect;

    public Animator anim;

    private float shootCooldown = 1f;
    private float shootTimer = 0f;

    private void Start()
    {

        if (shootButton) shootButton.onClick.AddListener(Shoot);
    }

    private void Update()
    {

        shootTimer += Time.deltaTime;

        UpdateTrajectoryMarker();
    }

    public void Shoot()
    {

        if (shootTimer < shootCooldown) return;

        if (ScoreManager.instance.Score > 0)
        {
            ScoreManager.instance.ChangeValue(-1);
        }
        else
        {
            return;
        }
   

        if (effect) effect.Play();



        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);


        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = shootingPoint.up * shootingForce;
        }


        shootTimer = 0f;
    }

    private void UpdateTrajectoryMarker()
    {

        Vector3 startPosition = shootingPoint.position;
        Vector3 velocity = shootingPoint.up * shootingForce;


        float timeToFall = (-velocity.y - Mathf.Sqrt(velocity.y * velocity.y - 2 * gravity * startPosition.y)) / gravity;

        if (timeToFall > 0)
        {

            Vector3 endPosition = startPosition + velocity * timeToFall;
            endPosition.y = 0;

         //   if (trajectoryMarker) trajectoryMarker.position = endPosition;
        }
    }


}
