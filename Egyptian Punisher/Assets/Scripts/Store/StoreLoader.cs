using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class StoreLoader : MonoBehaviour
{
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject wings;

    void Start()
    {
        int shieldState = PlayerPrefs.GetInt("shield", 0);
        int swordState = PlayerPrefs.GetInt("sword", 0);
        int wingsState = PlayerPrefs.GetInt("wings", 0);

        if (shieldState == 1)
        {
            shield.SetActive(true);
        }
        else
        {
            shield.SetActive(false);
        }

        if (swordState == 1)
        {
            sword.SetActive(true);
        }
        else
        {
            sword.SetActive(false);
        }

        if (wingsState == 1)
        {
            wings.SetActive(true);
        }
        else
        {
            wings.SetActive(false);
        }
    }
}
