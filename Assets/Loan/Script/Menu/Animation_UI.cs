using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_UI : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void PlayAnimation(string Metronome_Anim)
    {
        _animator.Play(Metronome_Anim);
    }
}
