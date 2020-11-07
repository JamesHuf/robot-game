using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float explosionForce = 4;
    private float radiusMultiplier = 5;
    private float size = 1f;
    private int baseDamage = 35;

    private IEnumerator Start()
    {
        // Call function to delete the explosion after a few seconds
        StartCoroutine(Wait());

        // wait one frame because some explosions instantiate debris which should then
        // be pushed by physics force
        yield return null;

        float multiplier = GetComponent<UnityStandardAssets.Effects.ParticleSystemMultiplier>().multiplier;

        float r = 10 * multiplier * radiusMultiplier * size;
        var cols = Physics.OverlapSphere(transform.position, r);
        var rigidbodies = new List<Rigidbody>();
        var targets = new List<ReactiveTarget>();
        foreach (var col in cols)
        {
            if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody) && col.gameObject.tag != "Laser")
            {
                rigidbodies.Add(col.attachedRigidbody);
            }
            if (col.gameObject.GetComponent<PlayerCharacter>() != null)
            {
                targets.Add(col.gameObject.GetComponent<PlayerCharacter>());
            } else if (col.gameObject.GetComponent<EnemyTarget>() != null)
            {
                targets.Add(col.gameObject.GetComponent<EnemyTarget>());
            }
        }

        // Damage each target in the radius
        foreach (var target in targets)
        {
            float damage = baseDamage * size;
            target.Hit((int)damage);
        }

        // Push each object in the radius
        foreach (var rb in rigidbodies)
        {
            rb.AddExplosionForce(explosionForce * multiplier, transform.position, r, 1 * multiplier, ForceMode.Impulse);
        }
    }

    // This function initialize a newly instantiated values
    public void Initialize(float size)
    {
        this.size = size;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.7f);
        Destroy(this.gameObject);
    }
}
