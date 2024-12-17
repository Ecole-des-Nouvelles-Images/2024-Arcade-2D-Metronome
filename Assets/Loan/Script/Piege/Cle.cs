using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CleTrap", menuName = "Traps/Cle Trap")]
public class Cle : PiegeData
{
    public float ExplosionRadius = 3f;
    private float ExplosionForce = 1200f;

    private void OnEnable()
    {
        CanFall = true;
        HasExploded = true;
    }

    public void Explode(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, ExplosionRadius);

        foreach (Collider2D hit in colliders)
        {
            RunnersControler runner = hit.GetComponent<RunnersControler>();
            if (runner != null)
            {
               Rigidbody2D rb = runner.GetComponent<Rigidbody2D>();
               if (rb != null)
               {
                   Vector2 forceDirection = (runner.transform.position - (Vector3)position).normalized;
                   rb.AddForce(forceDirection * ExplosionForce);
               }
            }
        }
    }
}
