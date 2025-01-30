using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDeactivateScreenChanger : MonoBehaviour
{
    [SerializeField] private string screenName;
    [SerializeField] private PoolableObject poolable;

    private UIManager uiManager;

    private void Start()
    {
        uiManager = ServiceLocator.GetService<UIManager>();
        poolable.onDisable += OnDisableChangeScreen;
    }

    private void OnDestroy()
    {
        poolable.onDisable -= OnDisableChangeScreen;
    }

    private void OnDisableChangeScreen(PoolableObject poolable)
    {
        uiManager.ChangeScreen(screenName);
    }
}
