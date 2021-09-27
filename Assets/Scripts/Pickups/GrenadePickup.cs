using UnityEngine;

public class GrenadePickup : CollectibleItem
{
    protected override void Activate(PlayerCharacter activatingPlayer)
    {
        // Give grenade to activating player
        activatingPlayer.AddGrenade();
    }
}
