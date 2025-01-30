using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class TheftController : MonoBehaviour
{
    [SerializeField] private Button theftBtn;
    [SerializeField] private bool canTheft = false;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask victimLayer;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem theftEffect;

    [Space]
    private ScoreManager scoreManager;
    private Collider[] nearbyColliders;


    private void Start()
    {
        theftBtn.onClick.AddListener(OnCLickTheft);
        scoreManager = ServiceLocator.GetService<ScoreManager>();
    }

    private void Update()
    {
        nearbyColliders = Physics.OverlapSphere(transform.position, radius, victimLayer);

        if (nearbyColliders.Length > 0)
        {
            TurnOnBtn();
        }
        else
        {
            TurnOffBtn();
        }
    }

    private void OnCLickTheft()
    {
        print("Клик");
        if (animator) animator.SetTrigger("IsAttack");

        foreach (Collider collider in nearbyColliders)
        {
            if (collider.TryGetComponent(out Inventory inventory))
            {
                scoreManager.UpdateScore(inventory.amountGold);
                inventory.amountGold = 0;
                inventory.GetHit();
            }
        }
        theftEffect.Play();
    }

    private void TurnOnBtn()
    {
        if (!canTheft)
        {
            theftBtn.gameObject.SetActive(true);
            canTheft = true;
        }
    }

    private void TurnOffBtn()
    {
        if (canTheft)
        {
            theftBtn.gameObject.SetActive(false);
            canTheft = false;
        }
    }

    private void OnDrawGizmos()
    {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
    }
}

