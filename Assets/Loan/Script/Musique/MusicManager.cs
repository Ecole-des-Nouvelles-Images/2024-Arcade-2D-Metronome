using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _defaultMusic;
    [SerializeField] private AudioClip _gameMusic;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musiqueSlider;
    [SerializeField] private Slider _sfxSlider;
    

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
        
        _audioSource = GetComponent<AudioSource>();

        if (_defaultMusic != null)
        {
            PlayMusic(_defaultMusic);
        }
    }

    private void Start()
    {
        _masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        _musiqueSlider.value = PlayerPrefs.GetFloat("MusiqueVolume", 0.75f);
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        ApplyVolumeSetting();
        
        _masterSlider.onValueChanged.AddListener(SetMasterVolume);
        _musiqueSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void ApplyVolumeSetting()
    {
        SetMasterVolume(_masterSlider.value);
        SetMusicVolume(_musiqueSlider.value);
        SetSFXVolume(_sfxSlider.value);
    }
    
    private void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    
    private void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("MusiqueVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusiqueVolume", volume);
    }

    private void SetSFXVolume(float volume)
    {
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void StopMusic()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Pause();
        }
    }

    public void ReprendMusic()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
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
