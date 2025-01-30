using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Animator collectAnim;
    [SerializeField] private Animator startAnim;
    [SerializeField] private Animator returnAnim;

    [Space]
    public ParticleSystem finishEffect;

    public static ScoreManager instance;

    [ReadOnly] public int score;
    public int results;
    public int Score { get { return score; } }

    public Action<int> onBalanceChange;

    public int goal;
    public TextMeshProUGUI goalTxt;
    public TextMeshProUGUI resultTxt;
    public TextMeshProUGUI resultTxt2;
    public TextMeshProUGUI LevelTxt;

    public GameObject spawner;

    public int curLvl;

    private void Awake()
    {
        print("Последний пройдены уровень " + PlayerPrefs.GetInt("lvl", 0));

        instance = this;
        if(startAnim) startAnim.SetTrigger("hint");
        if (spawner) spawner.SetActive(false);
    }

    private void OnEnable()
    {
        if (spawner) spawner.SetActive(false);

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName != "Menu")
        {
            string lastTwoChars = sceneName.Substring(System.Math.Max(0, sceneName.Length - 2));

            int lastLevel = int.Parse(lastTwoChars);

            if (LevelTxt) LevelTxt.text = $"LEVEL {lastTwoChars}";

            goal = lastLevel * 10;

            curLvl  = lastLevel;

        }

    



        if (goalTxt) goalTxt.text = $"Collect {goal} balls to win and \ngo to the next level";
       // score = PlayerPrefs.GetInt("score", 0);
        ServiceLocator.AddService(this);
    }

    private void OnDisable()
    {
        ServiceLocator.RemoveService(this);
    }

    [Button]
    private void add100()
    {
        ChangeValue(100);
        FinishGame();
    }

    public void ChangeValue(int value)
    {
        score += value; 
        onBalanceChange?.Invoke(score);
        //scoreInGameBfUpt += value;

        if (score >= goal) 
        {
            if (spawner) spawner.SetActive(true);
            if (finishEffect)finishEffect.Play();
           if (returnAnim) returnAnim.SetTrigger("hint");
            
        }

    }

    public void ShowFall()
    {
       if(collectAnim) collectAnim.SetTrigger("hint");
    }

    public void FinishGame()
    {
       // if (spawner) spawner.GetComponent<Spawer>().DestroyAllBalls(); 
       if(spawner) spawner.SetActive(false);
       if(resultTxt) resultTxt.text = $"{score}";
        if (resultTxt2) resultTxt2.text = $"{score}";
        results = score;
        PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score", 0) + score);
      //  if (PlayerPrefs.GetInt("record", 0) < results) PlayerPrefs.SetInt("record", results);
      //  if (results >= 100) PlayerPrefs.SetInt("goal5", PlayerPrefs.GetInt("goal5", 0) + 1);
      //  if (results >= 150) PlayerPrefs.SetInt("goal6", PlayerPrefs.GetInt("goal6", 0) + 1);
      //  if (results >= 200) PlayerPrefs.SetInt("goal7", PlayerPrefs.GetInt("goal7", 0) + 1);

        ServiceLocator.GetService<UIManager>().ChangeScreen("Win");
    //
    //    //PlayerPrefs.SetInt(sceneName, 1);
        Scene currentScene = SceneManager.GetActiveScene();
    //
    //    // Получаем название сцены
        string sceneName = currentScene.name;
    //
    //    // Получаем последние два символа
        string lastTwoChars = sceneName.Substring(System.Math.Max(0, sceneName.Length - 2));

        
    //
    //    // Выводим результат
    //    Debug.Log("Последние два символа: " + lastTwoChars);
    //
    //    // Преобразуем последние два символа в число (предполагая, что это номер уровня)
        int lastLevel = int.Parse(lastTwoChars);
    //
    //    if (results < lastLevel*10) return;
    //    // Сохраняем значение уровня в PlayerPrefs, если текущее значение меньше
        int currentLevel = PlayerPrefs.GetInt("lvl", 0);
        if (currentLevel < lastLevel)
        {
            PlayerPrefs.SetInt("lvl", lastLevel);
            PlayerPrefs.Save(); // Необходимо сохранить изменения
        }

        print("Прошел уровень  " + lastLevel); 

    }


    public int scoreInGameBfUpt;
    public void Finish(bool e)
    {

    }
    public void UpdateGame(int few)
    {

    }
    public void UpdateScore(int fw)
    {

    }
}
