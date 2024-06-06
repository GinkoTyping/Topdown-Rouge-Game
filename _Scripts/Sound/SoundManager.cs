using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // static 保证以下Instance变量只被创建一次
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource effectSource, musicSource;
    [SerializeField] private float volume;

    [Header("Common Audio")]
    [SerializeField] private AudioClip errorHint;

    [Header("UI Audio")]
    [SerializeField] private AudioClip buttonClick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        AudioListener.volume = volume;
    }

    public void PlaySound(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }

    public void StopSound()
    {
        effectSource.Stop();
    }

    public void Warning()
    {
        effectSource.PlayOneShot(errorHint);
    }

    public void ButtonClick()
    {
        effectSource.PlayOneShot(buttonClick);
    }
}
