using UnityEngine;

public class PlayerCharacter : ReactiveTarget
{
    [Header("References")]
    [Tooltip("Reference to the main camera used for the player")]
    [SerializeField] private Camera playerCamera = null;
    [Tooltip("Reference to the prefab used for the spawned grenade")]
    [SerializeField] private GameObject grenadePrefab = null;

    // Grenade Variables
    private float grenadeSpawnDistance = 1f;
    private int numGrenades = 3;
    private const int maxGrenades = 3;
    private const float grenadeSpeed = 12f;

    public override void Hit(int damage)
    {
        base.Hit(damage);
        new PlayerHealthChangedEvent(((float)health) / maxHealth).Fire();
    }

    protected override void Die()
    {
        //Debug.Break();
        new PlayerDeadEvent().Fire();
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
            new PlayerHealthChangedEvent(((float)health) / maxHealth).Fire();
        }
    }

    public void AddGrenade()
    {
        if (numGrenades < maxGrenades)
        {
            numGrenades++;
            new GrenadeCountChangedEvent(numGrenades).Fire();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Grenade") && numGrenades > 0)
        {
            numGrenades--;
            new GrenadeCountChangedEvent(numGrenades).Fire();

            GameObject grenade = Instantiate(grenadePrefab, playerCamera.transform.position + playerCamera.transform.forward *
                                    grenadeSpawnDistance, playerCamera.transform.rotation);
            Rigidbody rbody = grenade.GetComponent<Rigidbody>();
            rbody.AddForce(playerCamera.transform.forward * grenadeSpeed, ForceMode.Impulse);
        }
    }
}
