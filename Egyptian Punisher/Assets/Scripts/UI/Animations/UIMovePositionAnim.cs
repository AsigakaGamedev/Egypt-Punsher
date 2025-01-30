using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMovePositionAnim : MonoBehaviour, IScreenListener
{
    [SerializeField] private float moveDuration;
    [SerializeField] private Button btn;
    [SerializeField] private RectTransform point1;
    [SerializeField] private RectTransform point2;
    [SerializeField] private RectTransform target;

    private Transform nextPoint;

    void IScreenListener.OnScreenInit()
    {
        target.position = point1.position;

        btn.onClick.AddListener(() =>
        {
            if (nextPoint == point1)
            {
                MoveToPoint1();
            }
            else
            {
                MoveToPoint2();
            }
        });
    }

    private void MoveToPoint1()
    {
        target.DOMove(point1.position, moveDuration);
        nextPoint = point2;
    }

    private void MoveToPoint2()
    {
        target.DOMove(point2.position, moveDuration);
        nextPoint = point1;
    }
}
