using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRecordText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recordText;

    private void OnEnable()
    {
        recordText.text = PlayerPrefs.GetInt("record", 0).ToString();
    }
}
