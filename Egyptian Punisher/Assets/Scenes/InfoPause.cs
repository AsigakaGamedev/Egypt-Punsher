using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoPause : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void Start()
    {
        // �������� �������� �� �������� ����� � ������ Start
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            anim.SetTrigger("hint");
        }
    }

    private void OnEnable()
    {
        // ���������, ���� ����� �� "Menu", ���������� ��������
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            anim.SetTrigger("hint");
        }
    }
}
