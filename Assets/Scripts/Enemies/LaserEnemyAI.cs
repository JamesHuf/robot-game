using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { alive, dead };

public class LaserEnemyAI : BaseEnemyAI
{
    GameObject player;
    NavMeshAgent agent;

    private EnemyStates state = EnemyStates.alive;

    public GameObject laserbeamPrefab;
    private GameObject laserbeam;
    public float fireRate = 2.0f;
    private float nextFire = 0.0f;
    private AudioSource fireSound = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        fireSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyStates.alive)
        {
            // Move Enemy
            agent.SetDestination(player.transform.position);
            
            // Generate ray
            Ray ray = new Ray(transform.position, transform.forward);

            // Spherecast and determine if player is in front
            RaycastHit hit;
            if (Physics.SphereCast(ray, 0.75f, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<PlayerCharacter>())
                {
                    // Spherecast hit Player, fire Laser!
                    if(laserbeam == null && Time.time > nextFire)
                    {
                        fireSound.Play();
                        nextFire = Time.time + fireRate;
                        laserbeam = Instantiate(laserbeamPrefab) as GameObject;
                        laserbeam.transform.position = transform.TransformPoint(0, 1.5f, 1.5f);
                        laserbeam.transform.rotation = transform.rotation;
                    }
                }
            }
        } else
        {
            agent.isStopped = true;
        }
    }

    // On death: Change state to dead
    public override void Stop()
    {
        ChangeState(EnemyStates.dead);
    }

    public void ChangeState(EnemyStates state)
    {
        this.state = state;
    }
    public EnemyStates GetState()
    {
        return state;
    }
}
