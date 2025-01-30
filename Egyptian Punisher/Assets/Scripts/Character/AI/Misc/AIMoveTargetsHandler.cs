using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveTargetsHandler : MonoBehaviour
{
    [SerializeField] private TargetsContainer[] containers;

    public TargetsContainer[] Containers { get => containers; }

    private void OnEnable()
    {
        ServiceLocator.AddService(this);
    }

    private void OnDisable()
    {
        ServiceLocator.RemoveService(this);
    }
}

[System.Serializable]
public struct TargetsContainer
{
    public Transform[] Targets;
}
