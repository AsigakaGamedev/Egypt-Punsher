using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int amountGold;
    [SerializeField] private Animator animator;

    public void GetHit()
    {
        if (animator) animator.SetTrigger("GetHit");
    }
}
