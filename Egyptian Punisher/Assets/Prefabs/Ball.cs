using ETFXPEL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ball : MonoBehaviour
{
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private float destroyEffect;
    [SerializeField] private Animator anim;

    private Rigidbody rb;
    private Collider col;

    void Start()
    {
        // Получаем ссылки на компоненты Rigidbody и Collider
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Проверяем, коснулся ли объект слоя "Island"
        if (collision.gameObject.layer == LayerMask.NameToLayer("Island"))
        {
            // Обнуляем скорость
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Отключаем Rigidbody
            rb.isKinematic = true;

            // Делаем коллайдер триггером
            col.isTrigger = true;


        }

        if (collision.gameObject.CompareTag("Enemy"))
        {

            effectBoom.Play();
            collision.gameObject.GetComponent<HealthComponent>().Damage(50 * PlayerPrefs.GetInt("attack", 1));

            if (collision.gameObject.GetComponent<HealthComponent>().isDead)
            {
                print("����");
                //GetComponent<AICharacter>().anim.SetTrigger("fall");
                print("����� �������");
                DisableBall();

                //  CheckRecord.Instance.killed();

                anim.SetTrigger("die");


                Destroy(gameObject, 0.5f);


            }


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.instance.ChangeValue(1);

            Destroy(gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
           
            effectBoom.Play();
            other.GetComponent<HealthComponent>().Damage(50 * PlayerPrefs.GetInt("attack", 1));

            if (other.GetComponent<HealthComponent>().isDead)
            {
                print("����");
                //GetComponent<AICharacter>().anim.SetTrigger("fall");
                print("����� �������");
                DisableBall();

                //  CheckRecord.Instance.killed();

                anim.SetTrigger("die");


                Destroy(gameObject, 0.5f);


            }


        }
    }


    [SerializeField] private ParticleSystem effectBoom;


    private void DisableBall()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // ��������� ���������
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
    }

}
