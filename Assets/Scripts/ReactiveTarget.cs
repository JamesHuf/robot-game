using UnityEngine;

public abstract class ReactiveTarget : MonoBehaviour
{
    protected int maxHealth;
    protected int health;

    // Constructor
    // Parameter - health: determines the starting health and maxHealth of the target
    public void Initialize(int health)
    {
        maxHealth = health;
        this.health = health;
    }

    // Deal damage to the target
    // Parameter - damage: the amount of damage to deal to the targets health
    public virtual void Hit(int damage)
    {
        //Debug.Log(damage);
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    // This function is called once the entities health reaches 0
    // Purpose: To perform functionality such as sending out death notification
    // or starting a death animation
    abstract protected void Die();
}
