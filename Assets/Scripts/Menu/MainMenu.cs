using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName;

    [SerializeField]
    private GameObject quitButton, settingsButton;

    private void Start()
    {
        MusicPlayer.Instance.PlayMainMenuMusic();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

#if !UNITY_EDITOR
        Debug.unityLogger.logEnabled = false;
        settingsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
#endif

#if UNITY_STANDALONE
        quitButton.gameObject.SetActive(true);
        quitButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
#endif
    }

    public void StartButtonPressed()
    {
        SoundEffectPlayer.Instance.MenuSelectSound();
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitButtonPressed()
    {
        SoundEffectPlayer.Instance.MenuSelectSound();
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
