using UnityEngine;

public class HealthPickup : CollectibleItem
{
    private int value = 25;

    protected override void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            // apply health and remove the item
            player.FirstAid(value);
            Destroy(this.gameObject);
        }
    }
}
