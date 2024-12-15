using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDestroy : MonoBehaviour
{
    void Start()
    {
        Destroy(MusicManager.Instance.gameObject);
    }
}
