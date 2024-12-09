using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mage", menuName = "Runner/Mage")]
public class Mage : RunnerData
{
    public GameObject BarrierPrefab;
    private GameObject _currentBarrier;
    private bool _isActive;
    public override void ApplyPowerUp(RunnersControler runner)
    {
        if (_isActive == false)
        {
            _currentBarrier = Instantiate(BarrierPrefab, runner.transform.position, Quaternion.identity);
            _currentBarrier.transform.SetParent(runner.transform);
            _isActive = true;
        }
    }

    public override void RemovePowerUp(RunnersControler runner)
    {
        GameObject.Destroy(_currentBarrier);
        _currentBarrier = null;
        _isActive = false;
    }
}
