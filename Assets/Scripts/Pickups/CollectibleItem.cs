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

    protected virtual void OnTriggerEnter(Collider other){}
}
