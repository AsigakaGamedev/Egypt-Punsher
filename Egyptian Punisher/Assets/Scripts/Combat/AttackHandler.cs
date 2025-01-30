using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum AttackState { Cast, Attacked, CanAttack }

public class AttackHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool initOnStart;

    [Space]
    [SerializeField] private float castTime;
    [SerializeField] private float attackDelay;

    [Space]
    [SerializeField] private GameObject delayObj;
    [ShowIf(nameof(hasDelayUI)), SerializeField] private Slider delaySlider;
    [ShowIf(nameof(hasDelayUI)), SerializeField] private TextMeshProUGUI delayText;

    [Space]
    [SerializeField] private float neededValue;
    [SerializeField] private float yAttackOffset = 0;

    [Space]
    [SerializeField] private AttackCastEffect[] castEffects;
    [SerializeField] private AttackEffect[] attackEffects;

    [Space]
    [SerializeField] private AttackState state;

    private ObjectPoolingManager poolingManager;
    private Vector3 attackPoint;

    private bool hasDelayUI => delayObj != null;

    public float NeededValue { get => neededValue; }

    private void Start()
    {
        if (initOnStart) Init();
    }

    public void Init()
    {
        poolingManager = ServiceLocator.GetService<ObjectPoolingManager>();

        state = AttackState.CanAttack;

        foreach (AttackEffect effect in attackEffects)
        {
            effect.OnInit();
        }

        if (delayObj) delayObj.SetActive(false);
    }

    [Button]
    public void TryAttack()
    {
        TryAttack(Vector3.zero);
    }

    public bool TryAttack(Vector3 attackPoint)
    {
        attackPoint += new Vector3(0, yAttackOffset, 0);

        if (state == AttackState.CanAttack)
        {
            this.attackPoint = attackPoint;
            StartCast();
            return true;
        }

        return false;
    }

    private void StartCast()
    {
        state = AttackState.Cast;

        foreach (AttackCastEffect prefab in castEffects)
        {
            PoolableObject effect = poolingManager.GetPoolable(prefab.Prefab);
            effect.transform.position = attackPoint + prefab.Offset;
        }

        Invoke(nameof(Attack), castTime);
    }

    private void Attack()
    {
        print("Атака");
        foreach (AttackEffect effect in attackEffects)
        {
            effect.OnAttack(attackPoint);
        }

        if (animator) animator.SetTrigger("Attack");

        if (delayObj) StartCoroutine(EShowDelayUI());

        state = AttackState.Attacked;
        Invoke(nameof(EndAttack), attackDelay);
    }

    private void EndAttack()
    {
        state = AttackState.CanAttack;
    }

    private IEnumerator EShowDelayUI()
    {
        delayObj.SetActive(true);
        delaySlider.maxValue = attackDelay;

        float timer = attackDelay;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            delaySlider.value = timer;
            delayText.text = $"{(int)timer}";
            yield return null;
        }


        delayObj.SetActive(false);
    }
}

[System.Serializable]
public struct AttackCastEffect
{
    public PoolableObject Prefab;
    public Vector3 Offset;
}
