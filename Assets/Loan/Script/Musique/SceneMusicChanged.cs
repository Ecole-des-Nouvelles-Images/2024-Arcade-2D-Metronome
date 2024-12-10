using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicChanged : MonoBehaviour
{
    [SerializeField] private AudioClip _newAudioClip;

    private void Start()
    {
        MusicManager musicManager = FindObjectOfType<MusicManager>();

        if (musicManager != null && _newAudioClip != null)
        {
            musicManager.PlayMusic(_newAudioClip);
        }
    }
}
