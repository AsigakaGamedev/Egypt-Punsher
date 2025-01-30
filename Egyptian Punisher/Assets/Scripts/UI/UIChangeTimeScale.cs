using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChangeTimeScale : MonoBehaviour
{
    [SerializeField] private float activeTimeScale;
    [SerializeField] private float deactiveTimeScale;

    private void OnEnable()
    {
        Time.timeScale = activeTimeScale;
    }

    private void OnDisable()
    {
        Time.timeScale = deactiveTimeScale;
    }
}
