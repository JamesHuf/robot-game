using UnityEngine;

public class EnemyTarget : ReactiveTarget
{
    [Header("References")]
    [Tooltip("An array referencing all possible pickups spawned by this enemy")]
    [SerializeField] private GameObject[] pickups = null;

    [Header("Properties")]
    [SerializeField] private const int baseHealth = 20;

    [HideInInspector] public DeviceTrigger trigger = null;

    // This function is called once the enemies health reaches 0
    // Purpose: Sends out death notification and starts death animation
    protected override void Die()
    {
        // Alert AI of death
        BaseEnemyAI enemyAI = GetComponent<BaseEnemyAI>();
        if (enemyAI != null)
        {
            Messenger.Broadcast(GameEvent.ENEMY_DEAD);
            enemyAI.Stop();
        }

        // Play death animation if not a bomber
        Animator enemyAnimator = GetComponent<Animator>();
        BomberEnemyAI bomberAI = GetComponent<BomberEnemyAI>();
        if (enemyAnimator != null && bomberAI == null)
        {
            enemyAnimator.SetTrigger("Die");
        } else
        {
            DeadEvent();
        }
    }

    // This function is called after the death animation finishes
    // Purpose: Perform cleanup to remove dead enemy and spawn pickups
    private void DeadEvent()
    {
        // Randomly spawn a pickup
        if (Random.Range(0, 4) == 0)
        {
            GameObject pickup = Instantiate(pickups[Random.Range(0, pickups.Length)]) as GameObject;
            pickup.transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }

        // Close door if enemy dies within the trigger area
        if (trigger != null)
        {
            trigger.removeObject();
        }
        Destroy(this.gameObject);
    }
}
