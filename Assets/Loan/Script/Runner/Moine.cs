using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Moine", menuName = "Runner/Moine")]
public class Moine : RunnerData
{
    public float JumpMultiplier = 2f;

    public override void ApplyPowerUp(RunnersControler runner)
    {
        runner.JumpForce = runner.OriginalJump * JumpMultiplier;
    }

    public override void RemovePowerUp(RunnersControler runner)
    {
        runner.JumpForce = runner.OriginalJump;
    }
}
