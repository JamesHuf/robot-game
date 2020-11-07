using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private int score;
    public Text scoreValue;
    private const int baseScorePerKill = 1;
    private int scorePerKill = baseScorePerKill;

    private const float doublePointTime = 10.0f;
    [SerializeField] private Image doublePointImage = null;
    private int numDoublePointsActive = 0;

    [SerializeField] private Image grenadeIndicator1 = null;
    [SerializeField] private Image grenadeIndicator2 = null;
    [SerializeField] private Image grenadeIndicator3 = null;
    private Color usedGrenade;
    private Color availableGrenade;

    [SerializeField] private Text waveValue = null;
    public Image healthBar;
    public OptionsPopup optionsPopup;
    public Image crossHair;
    public GameOverScreen gameOverScreen;
    [SerializeField] private VictoryScreen victoryScreen = null;

    private int popupsOpen;

    // On pause: stop time and unlock the cursor
    private void PauseGame()
    {
        Time.timeScale = 0;
        crossHair.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // On resume: resume time and lock the cursor
    public void ResumeGame()
    {
        Time.timeScale = 1;
        crossHair.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // On enemy death: update the diplayed score
    private void OnEnemyDead()
    {
        score += scorePerKill;
        scoreValue.text = score.ToString();
    }

    // When player health changed: update the displayed health bar
    private void OnHealthChanged(float healthPercentage)
    {
        healthBar.fillAmount = healthPercentage;
        healthBar.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }

    // Activate double points effect on pickup
    private void OnDoublePoints()
    {
        StartCoroutine(ActivateDoublePoints(doublePointTime));
    }

    private void OnPopupOpened()
    {
        popupsOpen++;
        Messenger<int>.Broadcast(GameEvent.UI_POPUP_OPENED, popupsOpen);
    }

    private void OnPopupClosed()
    {
        popupsOpen--;
        Messenger<int>.Broadcast(GameEvent.UI_POPUP_CLOSED, popupsOpen);
    }

    private void OnPlayerDead()
    {
        PauseGame();
        gameOverScreen.Open();
    }

    private void OnRestartGame()
    {
        ResumeGame();
        SceneManager.LoadScene(0);
    }

    private void OnGameWon()
    {
        PauseGame();
        victoryScreen.Open(score);
    }

    private void OnWaveStart(int currWave, int maxWaves)
    {
        waveValue.text = currWave.ToString() + "/" + maxWaves.ToString();
    }

    // Update displayed grenade count if changed
    private void OnGrenadeCountChanged(int grenadeCount)
    {
        if (grenadeCount == 0)
        {
            grenadeIndicator1.color = usedGrenade;
            grenadeIndicator2.color = usedGrenade;
            grenadeIndicator3.color = usedGrenade;
        }
        else if (grenadeCount == 1)
        {
            grenadeIndicator1.color = availableGrenade;
            grenadeIndicator2.color = usedGrenade;
            grenadeIndicator3.color = usedGrenade;
        }
        else if (grenadeCount == 2)
        {
            grenadeIndicator1.color = availableGrenade;
            grenadeIndicator2.color = availableGrenade;
            grenadeIndicator3.color = usedGrenade;
        }
        else
        {
            grenadeIndicator1.color = availableGrenade;
            grenadeIndicator2.color = availableGrenade;
            grenadeIndicator3.color = availableGrenade;
        }
    }

    private void Awake()
    {
        Messenger.AddListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
        Messenger<float>.AddListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);
        Messenger.AddListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        Messenger.AddListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
        Messenger.AddListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.AddListener(GameEvent.RESTART_GAME, OnRestartGame);
        Messenger.AddListener(GameEvent.DOUBLE_POINTS, OnDoublePoints);
        Messenger.AddListener(GameEvent.GAME_WON, OnGameWon);
        Messenger<int, int>.AddListener(GameEvent.WAVE_STARTED, OnWaveStart);
        Messenger<int>.AddListener(GameEvent.GRENADE_COUNT_CHANGED, OnGrenadeCountChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
        // init score
        score = 0;
        scoreValue.text = score.ToString();

        // init wave counter
        waveValue.text = "0";

        // init healthBar
        healthBar.fillAmount = 1;
        healthBar.color = Color.green;

        // Capture cursor
        crossHair.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;

        // Hide menus
        optionsPopup.Close();
        gameOverScreen.Close();
        victoryScreen.Close();
        popupsOpen = 0;

        // Hide powerups
        doublePointImage.enabled = false;

        // Initialize grenades
        grenadeIndicator1.enabled = true;
        grenadeIndicator2.enabled = true;
        grenadeIndicator3.enabled = true;
        usedGrenade = grenadeIndicator1.color;
        usedGrenade.a = 0.4f;
        availableGrenade = grenadeIndicator1.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && popupsOpen == 0)
        {
            PauseGame();
            optionsPopup.Open();
        }
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
        Messenger<float>.RemoveListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);
        Messenger.RemoveListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        Messenger.RemoveListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.RemoveListener(GameEvent.RESTART_GAME, OnRestartGame);
        Messenger.RemoveListener(GameEvent.DOUBLE_POINTS, OnDoublePoints);
        Messenger.RemoveListener(GameEvent.GAME_WON, OnGameWon);
        Messenger<int, int>.RemoveListener(GameEvent.WAVE_STARTED, OnWaveStart);
        Messenger<int>.RemoveListener(GameEvent.GRENADE_COUNT_CHANGED, OnGrenadeCountChanged);
    }

    // Only activate/disactivate double points if there is no other instance running
    // This currently stacks the effect of the powerup - if two powerups score is quadrupled
    private IEnumerator ActivateDoublePoints(float time)
    {
        numDoublePointsActive++;
        if (numDoublePointsActive == 1)
        {
            scorePerKill = 2 * baseScorePerKill;
            doublePointImage.enabled = true;
        }

        yield return new WaitForSeconds(time);

        numDoublePointsActive--;
        if (numDoublePointsActive == 0)
        {
            scorePerKill = baseScorePerKill;
            doublePointImage.enabled = false;
        }
    }
}
