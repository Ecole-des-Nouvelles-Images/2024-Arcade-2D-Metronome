using UnityEngine;

public abstract class RunnerData : ScriptableObject
{
    public Sprite Sprite;
    public float MaxPower;
    
    public abstract void ApplyPowerUp  (RunnersControler runners);

    public virtual void RemovePowerUp(RunnersControler runners)
    {
        Debug.Log("Power Up Removed" + name);
    }

}