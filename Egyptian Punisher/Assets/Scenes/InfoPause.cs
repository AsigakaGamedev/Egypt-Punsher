using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoPause : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void Start()
    {
        // Проводим проверку на название сцены в методе Start
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            anim.SetTrigger("hint");
        }
    }

    private void OnEnable()
    {
        // Проверяем, если сцена не "Menu", активируем анимацию
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            anim.SetTrigger("hint");
        }
    }
}
