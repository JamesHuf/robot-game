using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BomberEnemyAI : BaseEnemyAI
{
    [Header("References")]
    [Tooltip("Reference to the prefab spawned for the explosion")]
    [SerializeField] private GameObject explosionPrefab = null;
    [Tooltip("Reference to the beep played by the bomber")]
    [SerializeField] private AudioSource beepSound = null;

    GameObject player;
    NavMeshAgent agent;

    private const float explosionSize = 0.6f;
    private const float detonationDistance = 2.0f;

    private const float beepDelay = 0.5f;

    private const int baseHealth = 20;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        EnemyTarget target = GetComponent<EnemyTarget>();
        if (target != null)
        {
            target.Initialize(baseHealth);
        }

        StartCoroutine(beepForever());
    }

    IEnumerator beepForever()
    {
        while (true)
        {
            yield return new WaitForSeconds(beepDelay);
            beepSound.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move Enemy
        agent.SetDestination(player.transform.position);

        // Determine if player is within detonationDistance - if yes, explode
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < detonationDistance)
        {
            ReactiveTarget self = GetComponent<ReactiveTarget>();
            self.Hit(999999);
        }
    }

    // On death: Explode
    public override void Stop()
    {
        Explode();
    }

    // Spawn explosion on location
    public void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.GetComponent<Explosion>().Initialize(explosionSize);
    }
}
