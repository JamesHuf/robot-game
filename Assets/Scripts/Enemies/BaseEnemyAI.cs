using UnityEngine;

public abstract class BaseEnemyAI : MonoBehaviour
{
    // This function is called once the entities health reaches 0
    // Purpose: To stop normal AI behavior and execute any special death behavior
    abstract public void Stop();
}
