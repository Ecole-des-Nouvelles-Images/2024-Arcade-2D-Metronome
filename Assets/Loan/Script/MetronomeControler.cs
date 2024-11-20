using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomeControler : MonoBehaviour
{
    private InputSysteme _inputSysteme;
    private Renderer _renderer;

    private void Start()
    {
        _inputSysteme = GetComponent<InputSysteme>();
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (_inputSysteme.Active == 1)
        {
            _renderer.material.color = Color.red;
            Invoke(nameof(Reset),2f);
        }
    }

    private void Reset()
    {
        _renderer.material.color = Color.white;
    }
}
