using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScoreText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private bool inGame = false;
    [SerializeField] private bool showResult = false;

    [SerializeField] private bool IsShop = false;

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = ServiceLocator.GetService<ScoreManager>();
        scoreManager.onBalanceChange += UpdateBalance;
        UpdateBalance();// scoreManager.Score);
    }

    private void OnDestroy()
    {
        if (scoreManager)
        {
            scoreManager.onBalanceChange -= UpdateBalance;
        }
    }

    [Button]
    private void UpdateBalance(int balance = 0)
    {


        if (balanceText) balanceText.text = $"{PlayerPrefs.GetInt("score", 0)}";
        if (inGame & balanceText) balanceText.text = $"{balance}";
        if (showResult) balanceText.text = scoreManager.results.ToString() ;
    }

    private void Update()
    {
        if (IsShop) balanceText.text = $"{PlayerPrefs.GetInt("score", 0)}";
    }
}
