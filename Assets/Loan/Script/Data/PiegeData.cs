using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "NewTrap", menuName = "Traps/New Trap")]
public abstract class PiegeData : ScriptableObject
{
    public Sprite PiegeSprite;
    public float Mass = 1f;
    public float Damage = 0f;
    public bool CanFall = false;
    public bool HasExploded = false;
    public float FallSpeed = 4f;

}
