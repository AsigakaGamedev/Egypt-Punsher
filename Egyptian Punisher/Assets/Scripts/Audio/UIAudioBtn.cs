using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAudioBtn : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private AudioManager audioManager;

    private Button btn;

    private void Start()
    {
        audioManager = AudioManager.Instance;

        btn = GetComponent<Button>();
        btn.onClick.AddListener((UnityEngine.Events.UnityAction)(() =>
        {
            if (clip) audioManager.PlaySound(clip);
        }));
    }
}
