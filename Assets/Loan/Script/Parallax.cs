using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Material _material;
    private float _distance;

    [Range(0f, 10f)] public float speed = 0.2f;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        _distance += Time.deltaTime * speed;
        _material.SetTextureOffset("_MainTex",Vector2.right * _distance);
    }
}
