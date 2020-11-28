using UnityEngine;

public abstract class ReactiveTarget : MonoBehaviour
{
    [Header("ReactiveTarget")]
    [Tooltip("The maximum health of the target and the amount they will spawn with")]
    [SerializeField] protected int maxHealth;
    protected int health;

    private void Start()
    {
        health = maxHealth;
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
