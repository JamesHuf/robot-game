using UnityEngine;

public class GrenadeShooter : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the main camera used for the player")]
    [SerializeField] private Camera playerCamera = null;
    [Tooltip("Reference to the prefab used for the spawned grenade")]
    [SerializeField] private GameObject grenadePrefab = null;

    private float spawnDistance = 1f;

    private int numGrenades = 3;
    private const int maxGrenades = 3;

    private float grenadeSpeed = 12f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Grenade") && numGrenades > 0)
        {
            numGrenades--;
            Messenger<int>.Broadcast(GameEvent.GRENADE_COUNT_CHANGED, numGrenades);

            GameObject grenade = Instantiate(grenadePrefab, playerCamera.transform.position + playerCamera.transform.forward * spawnDistance, playerCamera.transform.rotation);
            Rigidbody rbody = grenade.GetComponent<Rigidbody>();
            rbody.AddForce(playerCamera.transform.forward * grenadeSpeed, ForceMode.Impulse);
        }
    }

    public void AddGrenade()
    {
        if (numGrenades < maxGrenades)
        {
            numGrenades++;
            Messenger<int>.Broadcast(GameEvent.GRENADE_COUNT_CHANGED, numGrenades);
        }
    }
}
