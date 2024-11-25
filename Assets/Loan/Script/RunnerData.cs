using UnityEngine;

[CreateAssetMenu(fileName = "NewRunnerData", menuName = "ScriptableObjects/RunnerData")]
public class RunnerData : ScriptableObject
{
    public Sprite sprite;
    public float Speed;
    public float JumpForce;
    public int MaxPower;
    
}