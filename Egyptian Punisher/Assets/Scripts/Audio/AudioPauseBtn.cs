using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioPauseBtn : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI pauseTxt;
    [SerializeField] private string onAudioTxt = "OFF";
    [SerializeField] private string offAudioTxt = "ON";

    [Header("Objects")]
    [SerializeField] private GameObject onAudioObj;
    [SerializeField] private GameObject offAudioObj;

    [Header("Pause Options")]
    [SerializeField] private bool isPauseMusic = true;
    [SerializeField] private bool isPauseEffects = false; 

    private Button button;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;

        button = GetComponent<Button>();
        button.onClick.AddListener(OnPause);

        UpdateUI();
    }

    private void OnPause()
    {
        if (isPauseMusic)
        {
            audioManager.musicPaused = !audioManager.musicPaused;

            if (audioManager.musicPaused) audioManager.MusicSource.Pause();
            else audioManager.MusicSource.UnPause();
        }

        if (isPauseEffects)
        {
            audioManager.effectsPaused = !audioManager.effectsPaused;

            if (audioManager.effectsPaused) audioManager.EffectsSource.volume = 0;
            else audioManager.EffectsSource.volume = 1;     
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        bool isAllPaused = (isPauseMusic && audioManager.musicPaused) ||
                         (isPauseEffects && audioManager.effectsPaused);

        if (pauseTxt) pauseTxt.SetText(isAllPaused ? onAudioTxt : offAudioTxt);

        if (onAudioObj) onAudioObj.SetActive(!isAllPaused);
        if (offAudioObj) offAudioObj.SetActive(isAllPaused);
    }
}
