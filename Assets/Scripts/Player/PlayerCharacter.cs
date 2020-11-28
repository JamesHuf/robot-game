public class PlayerCharacter : ReactiveTarget
{
    public override void Hit(int damage)
    {
        base.Hit(damage);
        Messenger<float>.Broadcast(GameEvent.HEALTH_CHANGED, ((float)health) / maxHealth);
    }

    protected override void Die()
    {
        //Debug.Break();
        Messenger.Broadcast(GameEvent.PLAYER_DEAD);
    }

    // Heal the player by a given amount
    // This function prevents overhealing for more than the maxHealth
    // Parameter - healthAdded: the amount of health to heal
    public void FirstAid(int healthAdded)
    {
        if (health < maxHealth)
        {
            health += healthAdded;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            Messenger<float>.Broadcast(GameEvent.HEALTH_CHANGED, ((float)health) / maxHealth);
        }
    }
}
