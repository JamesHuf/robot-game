using UnityEngine;

public abstract class BasePopup : MonoBehaviour
{
    // Code run when popup is opened
    public virtual void Open()
    {
        this.gameObject.SetActive(true);
        new PopupOpenedEvent().Fire();
    }

    // Code run when popup is closed
    public virtual void Close()
    {
        this.gameObject.SetActive(false);
        new PopupClosedEvent().Fire();
    }

    public bool IsActive()
    {
        return this.gameObject.activeSelf;
    }
}
