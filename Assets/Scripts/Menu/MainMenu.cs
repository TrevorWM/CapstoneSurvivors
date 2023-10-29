using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName;

    private void Start()
    {
        MusicPlayer.Instance.PlayMainMenuMusic();
        //Debug.unityLogger.logEnabled = false;
    }

    public void StartButtonPressed()
    {
        SoundEffectPlayer.Instance.MenuSelectSound();
        SceneManager.LoadScene(gameSceneName);
    }
}
