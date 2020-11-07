using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab = null;
    private float explosionSize = 1.0f;
    const float explosionDelay = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
    }

    // Instantiate explosion on location and destroy self
    void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.GetComponent<Explosion>().Initialize(explosionSize);
        Destroy(this.gameObject);
    }

    // Explode after explosionDelay
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }
}
