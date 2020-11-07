using UnityEngine;

public class DoublePoints : CollectibleItem
{
    protected override void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            // Broadcast activation if hit by player
            Messenger.Broadcast(GameEvent.DOUBLE_POINTS);
            Destroy(this.gameObject);
        }
    }
}
