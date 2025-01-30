using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UISceneChanger : MonoBehaviour
{
    [Scene, SerializeField] private string[] levels;

    [Space]
    [SerializeField] private bool menuBtn;

    [Scene, SerializeField] private string sceneName;

    [Space]
    [SerializeField] private float delay;

    [Space]
    [SerializeField] private bool isRetry = false;
    [SerializeField] private bool isLvls = false;
    [SerializeField] private bool isNext = false;
    [SerializeField] private bool isNextScene = false;

    [Space]
    [SerializeField] private bool isLvl = false;
    [SerializeField] private bool lvl1 = false;
    [SerializeField] private Image locked;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI2;
    [SerializeField] private Button btn1;

    private Button negowbogebwobegwe;
    private bool wljbgewbogewbgwe;

    private void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnBtnClick()
    {
        if (isNext)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;

            string lastTwoChars = sceneName.Substring(System.Math.Max(0, sceneName.Length - 2));

            int lastLevel = int.Parse(lastTwoChars);

            string wefewfefwf = $"Game {lastLevel + 1}";

            try
            {
                SceneManager.LoadScene(wefewfefwf);
            }
            catch (System.Exception ex)
            {
                wefewfefwf = $"Game {lastLevel}";
                SceneManager.LoadScene(wefewfefwf);
            }
            return;
        }

        if (isLvls)
        {
            try
            {
                // PlayerPrefs.SetInt("played", 1);
                int sceneIndex = PlayerPrefs.GetInt("lvl", 0);
                SceneManager.LoadScene(levels[sceneIndex]);
                return;
            }
            catch
            {
                int sceneIndex = 19;
                SceneManager.LoadScene(levels[sceneIndex]);
                return;
            }

        }

        if (isRetry)
        {
            Scene herwegobgewgebwogbw = SceneManager.GetActiveScene();
            sceneName = herwegobgewgebwogbw.name;
        }
        if (isNextScene)
        {

        }

        if (wljbgewbogewbgwe) return;

        wljbgewbogewbgwe = true;
        Invoke(nameof(ChangeScene), delay);
    }

    private void Start()
    {
      

        if (menuBtn)
        {
            //if (PlayerPrefs.GetInt("played", 0) > 0)
            //{
           //     sceneName = "Game 2";
            //}
        }

        for (int i = 0; i < sceneName.Length; i++)
        {
            // print(sceneName[i]);
        }

        negowbogebwobegwe = GetComponent<Button>();
        negowbogebwobegwe.onClick.AddListener(OnBtnClick);


        if (isLvl)
        {
            if (btn1) btn1.onClick.AddListener(OnBtnClick);

            print(sceneName);

            

            string sceneId = sceneName.Substring(sceneName.Length - 2);

            int lastLevel = int.Parse(sceneId);

            textMeshProUGUI.text = $"{lastLevel}";


            

            if (lvl1)
            {
                locked.gameObject.SetActive(false);
                negowbogebwobegwe.interactable = true;

                return;
            }

            if (PlayerPrefs.GetInt("lvl", 0) + 1 >= lastLevel)
            {
                locked.gameObject.SetActive(false);

                negowbogebwobegwe.interactable = true;
            }
            else
            {
                locked.gameObject.SetActive(true);

                negowbogebwobegwe.interactable = false;

                ColorBlock wouegoegwbwefbw = negowbogebwobegwe.colors;

                wouegoegwbwefbw.disabledColor = wouegoegwbwefbw.normalColor;
                negowbogebwobegwe.colors = wouegoegwbwefbw;
            }
        }

        if (isLvls)
        {
            int sceneIndex = PlayerPrefs.GetInt("lvl", 1);
            print($"Level {sceneIndex}");
            

            lvlTxt.text = $"LEVEL{sceneIndex + 1}";
        }
    }

    [SerializeField] private TextMeshProUGUI lvlTxt;
}
