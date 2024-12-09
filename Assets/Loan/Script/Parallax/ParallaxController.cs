using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private Transform _cameraTransform;
    private Vector3 _cameraStartPosition;
    private float _distance;
    
    private GameObject[] _backgrounds;
    private Material[] _materials;
    private float [] _backgroundSpeeds;
    
    private float _farthestBackground;
    
    [Range(0.01f, 1f)]
    public float ParallaxSpeed;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _cameraStartPosition = _cameraTransform.position;
        
        int backCount = transform.childCount;
        _materials = new Material[backCount];
        _backgroundSpeeds = new float[backCount];
        _backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            _backgrounds[i] = transform.GetChild(i).gameObject;
            _materials[i] = _backgrounds[i].GetComponent<Renderer>().material;
        }
        
        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++)
        {
            if ((_backgrounds[i].transform.position.z - _cameraTransform.position.z) > _farthestBackground)
            {
                _farthestBackground = _backgrounds[i].transform.position.z - _cameraTransform.position.z;
            }
        }

        for (int i = 0; i < backCount; i++)
        {
            _backgroundSpeeds[i] = 1 - (_backgrounds[i].transform.position.z - _cameraTransform.position.z) / _farthestBackground;
        }
    }

    private void LateUpdate()
    {
        _distance = _cameraTransform.position.x - _cameraStartPosition.x;
        transform.position = new Vector3(_cameraTransform.position.x, 0, 0);

        for (int i = 0; i < _backgrounds.Length; i++)
        {
            float speed = ParallaxSpeed * _backgroundSpeeds[i];
            _materials[i].SetTextureOffset("_MainTex", new Vector2(_distance,0)*speed);
        }
    }
}
