using UnityEngine;

public class GrenadePickup : CollectibleItem
{
    protected override void OnTriggerEnter(Collider other)
    {
        GrenadeShooter shooter = other.GetComponentInChildren<GrenadeShooter>();
        if (shooter != null)
        {
            // Give grenade and remove the pickup
            shooter.AddGrenade();
            Destroy(this.gameObject);
        }
    }
}
