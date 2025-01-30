using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewMover : MonoBehaviour
{
    public List<GameObject> objects; // ������ �������� ��� ��������������
    public Button leftButton;       // ������ ��� �������� �����
    public Button rightButton;      // ������ ��� �������� ������

    private int currentIndex = 0;   // ������ �������� ��������� �������

    void Start()
    {
        // ���������, ��� ���� �� ���� ������ ����
        if (objects == null || objects.Count == 0)
        {
            Debug.LogError("Objects list is empty!");
            return;
        }

        // ������������� ��������: ������� ������ ������
        UpdateObjects();

        // ���������� ������������ ������� ������
        leftButton.onClick.AddListener(MoveLeft);
        rightButton.onClick.AddListener(MoveRight);
    }

    // �������������� �����
    public void MoveLeft()
    {
        if (objects.Count == 0) return;

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = objects.Count - 1; // ������� � ���������� �������
        }

        UpdateObjects();
    }

    // �������������� ������
    public void MoveRight()
    {
        if (objects.Count == 0) return;

        currentIndex++;
        if (currentIndex >= objects.Count)
        {
            currentIndex = 0; // ������� � ������� �������
        }

        UpdateObjects();
    }

    // ���������� ��������� ��������
    private void UpdateObjects()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i] != null)
            {
                objects[i].SetActive(i == currentIndex);
            }
        }
    }
}
