using UnityEngine;

public class DoublePoints : CollectibleItem
{
    protected override void Activate(PlayerCharacter activatingPlayer)
    {
        new DoublePointsEvent().Fire();
    }
}
