using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PyramidTrigger : MonoBehaviour
{
    [SerializeField] private GameObject sphereCamera;
    [SerializeField] private GameObject clickScreen;
    [SerializeField] private Button clickBtn;
    [SerializeField] private GameObject[] sphereStages;
    [SerializeField] private GameObject ballStage;
    [SerializeField] private ParticleSystem changeStageEffect;
    [SerializeField] private GameObject smallBallPrefab;
    [SerializeField] private int smallBallsCount = 10;
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int clicksPerStage = 10;
    [SerializeField] private Transform spawnIn;

    private int currentStageIndex = 0;
    private int currentClickCount = 0;
    private bool isActivated = false;

    public Animator animl;
    public Animator ballStageAnimator; // Добавляем Animator для ballStage

    private void Awake()
    {
        sphereCamera.SetActive(false);
        clickScreen.SetActive(false);
        clickBtn.onClick.AddListener(OnClick);

        //if (ballStage != null)
       // {
        //    ballStageAnimator = ballStage.GetComponent<Animator>(); // Получаем Animator ballStage
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sphereCamera.SetActive(true);
            clickScreen.SetActive(true);
            isActivated = true;
        }
    }

    private void OnClick()
    {
        if (!isActivated || currentStageIndex >= sphereStages.Length)
            return;

        currentClickCount++;

        // Запуск анимации тряски при каждом клике
        if (ballStageAnimator != null)
        {
            ballStageAnimator.SetTrigger("shake");
        }

        // Получаем текущий поворот
        Quaternion currentRotation = ballStage.transform.rotation;

        // Генерируем небольшой случайный угол (от -5 до 5 градусов по каждому из направлений)
        float randomX = Random.Range(-5f, 5f);
        float randomY = Random.Range(-5f, 5f);
        float randomZ = Random.Range(-5f, 5f);

        // Создаём новый небольшой поворот
        Quaternion randomRotation = Quaternion.Euler(randomX, randomY, randomZ);

        // Применяем новый поворот, умножая его на текущий
        ballStage.transform.rotation = currentRotation * randomRotation;


        if (currentClickCount >= clicksPerStage)
        {
            currentClickCount = 0;

            if (currentStageIndex > 0)
            {
                sphereStages[currentStageIndex - 1].SetActive(false);
            }

            if (currentStageIndex < sphereStages.Length)
            {
                sphereStages[currentStageIndex].SetActive(true);
                changeStageEffect.transform.position = sphereStages[currentStageIndex].transform.position;
                changeStageEffect.Play();
            }

            currentStageIndex++;

            if (currentStageIndex >= sphereStages.Length)
            {
                StartCoroutine(DestroyFinalStage());
                ScoreManager.instance.ShowFall();
            }
        }
    }

    private IEnumerator DestroyFinalStage()
    {
        GameObject finalStage = sphereStages[sphereStages.Length - 1];
        if (finalStage != null)
        {
            Destroy(finalStage);
            ScatterSmallBalls(finalStage.transform.position);
            ballStage.SetActive(false);
        }

        yield return new WaitForSeconds(Random.Range(0.5f, 1f));

        sphereCamera.SetActive(false);
        clickScreen.SetActive(false);
        isActivated = false;
        gameObject.SetActive(false);

        animl.SetTrigger("shake");
        animl.speed = 0.4f;

        Invoke(nameof(FallIsland), 30);
    }

    public void FallIsland()
    {
        animl.SetTrigger("fall");
    }

    private void ScatterSmallBalls(Vector3 origin)
    {
        for (int i = 0; i < smallBallsCount; i++)
        {
            GameObject smallBall = Instantiate(smallBallPrefab, origin, Random.rotation);
            Rigidbody rb = smallBall.GetComponent<Rigidbody>();
            smallBall.transform.SetParent(spawnIn);

            if (rb != null)
            {
                Vector3 randomDirection = Random.insideUnitSphere;
                randomDirection.y = Mathf.Abs(randomDirection.y) + 0.5f;
                rb.AddForce(randomDirection * explosionForce, ForceMode.Impulse);
                rb.AddExplosionForce(explosionForce, origin, explosionRadius);
            }
        }
    }

}