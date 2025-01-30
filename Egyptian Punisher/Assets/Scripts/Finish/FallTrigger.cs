using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    private UIManager uiManager;

    private void Start()
    {
        uiManager = ServiceLocator.GetService<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger fall");
        if (other.CompareTag("Player"))
        {
            ServiceLocator.GetService<UIManager>().ChangeScreen("Lose");
            //other.GetComponent<HealthComponent>().Damage(25);
        }
    }
}
