using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private bool deactivateOnDie;
    [SerializeField] private float deactivateDelay = 2;

    [Space]
    [SerializeField] private Animator animator;
    [SerializeField] private float timeAnimation;
    [SerializeField] private UIProgressBar healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [Space]
    [ReadOnly, SerializeField] private float health;

    [Space]
    [SerializeField] private ParticleSystem gotDamageEffect;

    public bool isDead;


    public Action<float> onDamage;
    public Action onDie;

    private UIManager uiManager;

    private void OnEnable()
    {
        if (TryGetComponent(out AICharacter aI))
        {
            aI.enabled = true;
        }
        if (TryGetComponent(out Collider coll))
        {
            coll.enabled = true;
        }
        //if (TryGetComponent(out Rigidbody rb))
        //{
        //    rb.constraints = RigidbodyConstraints.None;
        //}

        isDead = false;
    }

    private void Start()
    {
        uiManager = ServiceLocator.GetService<UIManager>();
        health = maxHealth;
        if (healthBar) healthBar.SetMaxValue(health);
        if (healthText) healthText.text = $"{health}/{maxHealth}";
    }

    public void Damage(float damage)
    {
        if (isDead) return;

        health -= damage;
        onDamage?.Invoke(health);
        if (healthBar) healthBar.SetValue(health);
        if (healthText) healthText.text = $"{health}/{maxHealth}";
        Invoke("GotDamage", timeAnimation);

        if (health <= 0)
        {
            if (deactivateOnDie) Invoke(nameof(Deactivate), deactivateDelay);

            if (animator) animator.SetTrigger("die");

            if (transform.GetComponent<AICharacter>() != null) gotDamageEffect.Play();

            onDie?.Invoke();
            if (healthBar) uiManager.ChangeScreen("Lose");

            if (TryGetComponent(out AICharacter aI))
            {
                aI.enabled = false;
            }
            if (TryGetComponent(out Collider coll))
            {
                coll.enabled = false;
            }
            if (TryGetComponent(out Rigidbody rb))
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            enabled = false;
            isDead = true;
        }
    }

    private void GotDamage()
    {
        if (gotDamageEffect) gotDamageEffect.Play();
        if (animator) animator.SetTrigger("GotDamage");
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
