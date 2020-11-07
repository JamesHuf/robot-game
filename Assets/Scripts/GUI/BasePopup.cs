using UnityEngine;

public abstract class BasePopup : MonoBehaviour
{
    // Code run when popup is opened
    public virtual void Open()
    {
        this.gameObject.SetActive(true);
        Messenger.Broadcast(GameEvent.POPUP_OPENED);
    }

    // Code run when popup is closed
    public virtual void Close()
    {
        this.gameObject.SetActive(false);
        Messenger.Broadcast(GameEvent.POPUP_CLOSED);
    }

    public bool IsActive()
    {
        return this.gameObject.activeSelf;
    }
}
