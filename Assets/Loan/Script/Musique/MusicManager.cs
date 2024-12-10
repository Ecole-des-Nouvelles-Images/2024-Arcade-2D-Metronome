using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _defaultMusic;
    [SerializeField] private AudioClip _gameMusic;
    

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        _audioSource = GetComponent<AudioSource>();

        if (_defaultMusic != null)
        {
            PlayMusic(_defaultMusic);
        }
    }

    public void StopMusic()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }

    public void PlayMusic(AudioClip newClip)
    {
        if (_audioSource.clip != newClip)
        {
            _audioSource.clip = newClip;
            _audioSource.Play();
        }
    }
}
