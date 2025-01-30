using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewMover : MonoBehaviour
{
    public List<GameObject> objects; // Список объектов для перелистывания
    public Button leftButton;       // Кнопка для листания влево
    public Button rightButton;      // Кнопка для листания вправо

    private int currentIndex = 0;   // Индекс текущего активного объекта

    void Start()
    {
        // Убедиться, что хотя бы один объект есть
        if (objects == null || objects.Count == 0)
        {
            Debug.LogError("Objects list is empty!");
            return;
        }

        // Инициализация объектов: активен только первый
        UpdateObjects();

        // Назначение обработчиков событий кнопок
        leftButton.onClick.AddListener(MoveLeft);
        rightButton.onClick.AddListener(MoveRight);
    }

    // Перелистывание влево
    public void MoveLeft()
    {
        if (objects.Count == 0) return;

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = objects.Count - 1; // Переход к последнему объекту
        }

        UpdateObjects();
    }

    // Перелистывание вправо
    public void MoveRight()
    {
        if (objects.Count == 0) return;

        currentIndex++;
        if (currentIndex >= objects.Count)
        {
            currentIndex = 0; // Переход к первому объекту
        }

        UpdateObjects();
    }

    // Обновление состояния объектов
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
