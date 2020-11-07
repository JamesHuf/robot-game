using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] devices = null;
    private int objectsInTrigger = 0;

    // Activate device if an entity enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Projectile")) {
            objectsInTrigger++;

            // Allow enemies to remove themselves if killed in trigger
            EnemyTarget target = other.gameObject.GetComponent<EnemyTarget>();
            if (target != null)
            {
                target.trigger = this;
            }

            // Open doors in trigger
            if (objectsInTrigger == 1)
            {
                foreach (GameObject device in devices)
                {
                    DoorControl door = device.GetComponent<DoorControl>();
                    if (door != null)
                    {
                        door.Operate();
                    }
                }
            }
        }
    }

    // Deactivate device if no entities remain in the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Projectile"))
        {
            objectsInTrigger--;

            // Stop enemies removing themselves after leaving trigger
            EnemyTarget target = other.gameObject.GetComponent<EnemyTarget>();
            if (target != null)
            {
                target.trigger = null;
            }

            // Close doors in trigger
            if (objectsInTrigger == 0)
            {
                foreach (GameObject device in devices)
                {
                    DoorControl door = device.GetComponent<DoorControl>();
                    if (door != null)
                    {
                        door.Operate();
                    }
                }
            }
        }
    }

    // Allow enemies to remove themselves from the zone if killed while inside it
    public void removeObject()
    {
        objectsInTrigger--;
        // Close doors in trigger
        if (objectsInTrigger == 0)
        {
            foreach (GameObject device in devices)
            {
                DoorControl door = device.GetComponent<DoorControl>();
                if (door != null)
                {
                    door.Operate();
                }
            }
        }
    }
}
