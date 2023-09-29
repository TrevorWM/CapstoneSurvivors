using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public static SoundEffectPlayer Instance { get; private set; }

    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip basicHit, fireballHit, fireballShoot;

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

    public void BasicHitSound()
    {
        source.clip = basicHit;
        source.Play();
    }
    public void FireballHitSound()
    {
        source.clip = fireballHit;
        source.Play();
    }
    public void FireballShootSound()
    {
        source.clip = fireballShoot;
        source.Play();
    }


}