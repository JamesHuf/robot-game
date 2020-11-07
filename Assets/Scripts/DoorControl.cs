using UnityEngine;

public class DoorControl : MonoBehaviour
{
    private AudioSource openSound = null;
    [SerializeField] private Vector3 openPos = new Vector3();
    [SerializeField] private Vector3 closedPos = new Vector3();

    private bool doorIsOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        openSound = GetComponent<AudioSource>();
    }

    // Open or close the door
    public void Operate()
    {
        openSound.Play();
        if (doorIsOpen)
        {
            iTween.MoveTo(this.gameObject, closedPos, 3);
        } else
        {
            iTween.MoveTo(this.gameObject, openPos, 3);
        }
        doorIsOpen = !doorIsOpen;
    }
}
