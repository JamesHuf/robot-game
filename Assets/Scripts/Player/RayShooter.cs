using UnityEngine;

public class RayShooter : MonoBehaviour
{
    private new Camera camera;
    [SerializeField] private GameObject sparkPrefab = null;

    [SerializeField] private AudioSource fireSound = null;

    private const int damage = 20;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIController.PopupsOpen && Input.GetMouseButtonDown(0))
        {
            fireSound.Play();

            Vector3 point = new Vector3(camera.pixelWidth / 2, camera.pixelHeight / 2, 0);
            Ray ray = camera.ScreenPointToRay(point);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Spawn sparks
                Instantiate(sparkPrefab, hit.point, Quaternion.identity);

                GameObject hitObject = hit.transform.gameObject;
                EnemyTarget target = hitObject.GetComponent<EnemyTarget>();
                // is this object our Enemy?
                if (target != null)
                {
                    target.Hit(damage);
                }

                // If projectile destroy it
                if (hitObject.CompareTag("Projectile"))
                {
                    Destroy(hitObject);
                }
            }
        }
    }
}
