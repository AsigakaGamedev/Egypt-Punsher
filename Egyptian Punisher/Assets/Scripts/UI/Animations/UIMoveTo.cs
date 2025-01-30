using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveTo : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float duration = 1;

    private Vector3 startPos;

    private void OnEnable()
    {
        startPos = transform.position;
        transform.DOMove(target.position, duration);
    }

    private void OnDisable()
    {
        transform.position = startPos;
    }
}
