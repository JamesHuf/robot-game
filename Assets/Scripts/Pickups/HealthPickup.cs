using UnityEngine;

public class HealthPickup : CollectibleItem
{
    private const int healingValue = 25;

    protected override void Activate(PlayerCharacter activatingPlayer)
    {
        activatingPlayer.FirstAid(healingValue);
    }
}
