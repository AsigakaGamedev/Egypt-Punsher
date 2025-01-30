using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseBg : MonoBehaviour
{
    [SerializeField] private int bgIndex;
    private Button useBtn;

    void Start()
    {
        useBtn = GetComponent<Button>();
        useBtn.onClick.AddListener(changeBg);
    }

    private void changeBg()
    {
        PlayerPrefs.SetInt("CurrentBG", bgIndex);
    }
}
