using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip level1Music;

    /// <summary>
    /// REF: https://gamedev.stackexchange.com/a/34879
    /// </summary>
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance!");
            return;
        }

        Instance = this;
    }

    public void PlayLevel1Music()
    {
        source.clip = level1Music;
        source.Play();
    }
}
