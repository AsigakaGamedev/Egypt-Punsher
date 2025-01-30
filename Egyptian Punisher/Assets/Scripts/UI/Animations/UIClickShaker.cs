using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIClickShaker : MonoBehaviour, IScreenListener
{
    [SerializeField] private float duration = 1;
    [SerializeField] private float strength = 0.5f;

    private Button btn;

    void IScreenListener.OnScreenInit()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            transform.DOShakeScale(duration, strength);
        });
    }
}
