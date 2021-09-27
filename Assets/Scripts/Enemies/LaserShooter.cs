using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    private float speed = 6f;
    private int damage = 20;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    // On collision: deal damage if hit player and destroy self
    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            player.Hit(damage);
        }
        Destroy(this.gameObject);
    }
}
