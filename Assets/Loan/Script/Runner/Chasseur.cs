using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chasseur", menuName = "Runner/Chasseur")]
public class Chasseur : RunnerData
{
    public float SpeedMultiplier = 2f;
    

    public override void ApplyPowerUp(RunnersControler runner)
    {
        runner.Speed = runner.OriginalSpeed * SpeedMultiplier;
    }

    public override void RemovePowerUp(RunnersControler runner)
    {
        runner.Speed = runner.OriginalSpeed;
    }
}
