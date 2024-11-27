using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoteTrap", menuName = "Traps/Note Trap")]
public class Note : PiegeData
{
    private void OnEnable()
    {
        CanFall = true;
        Damage = 1f;
    }
}
