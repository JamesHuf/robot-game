using UnityEngine;

public class OptionsPopup : BasePopup
{
    public UIController controller;

    // Close game if exit button is clicked
    public void OnExitGameButton()
    {
        Application.Quit();
    }

    // Resume game if return button is clicked
    public void OnReturnToGameButton()
    {
        Close();
        controller.ResumeGame();
    }
}
