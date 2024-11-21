using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetronomeControler : MonoBehaviour
{
    private InputSysteme _inputSysteme;
    private Image _image;

    private void Start()
    {
        _inputSysteme = GetComponent<InputSysteme>();
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if (_inputSysteme.Active == 1)
        {
            _image.color = new Color(1, 0, 0, 0.5f);
            Invoke(nameof(Reset),2f);
        }
    }

    private void Reset()
    {
        _image.color = new Color(0, 1,0, 0.5f);
    }
}
