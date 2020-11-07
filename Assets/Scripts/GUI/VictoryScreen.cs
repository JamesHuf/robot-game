using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VictoryScreen : BasePopup
{
    [SerializeField] private Text scoreDisplay = null;

    // Initialize score text when opened
    public void Open(int score)
    {
        base.Open();
        scoreDisplay.text = "Your Score: " + score;
    }

    // Exit the game if related button clicked
    public void OnExitGameButton()
    {
        Application.Quit();
    }

    // Restart the game if related button clicked
    public void OnStartAgainButton()
    {
        Messenger.Broadcast(GameEvent.RESTART_GAME);
    }
}
