using UnityEngine;

public class GameOverScreen : BasePopup
{
    // Close the game if exit button is clicked
    public void OnExitGameButton()
    {
        Application.Quit();
    }

    // Broadcast message to restart game if restart button is clicked
    public void OnStartAgainButton()
    {
        new GameRestartEvent().Fire();
    }
}
