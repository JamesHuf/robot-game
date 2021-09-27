using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectibleItem : MonoBehaviour
{
    // Rotate the pickup to add interest and make more noticable
    void Update()
    {
        transform.Rotate(0, 80 * Time.deltaTime, 0);
    }

    protected virtual void Activate(PlayerCharacter activatingPlayer){}

    protected void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            // Broadcast activation if hit by player
            Activate(player);
            Destroy(this.gameObject);
        }
    }
}
