using UnityEngine;

[CreateAssetMenu(fileName = "NewPiegeData", menuName = "ScriptableObjects/PiegeData")]
public class PiegeData : ScriptableObject
{
    public Sprite Sprite;
    public float Damage;
    public float Mass;
    public Vector2 ColliderSize;
}
