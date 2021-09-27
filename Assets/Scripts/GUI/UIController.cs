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

    public static bool PopupsOpen { get; private set; }
    private int numPopupsOpen;
    int NumPopupsOpen
    { get { return numPopupsOpen; }
      set { numPopupsOpen = value; PopupsOpen = numPopupsOpen > 0; }
    }

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
    private void OnEnemyDead(EnemyDeadEvent ede)
    {
        score += scorePerKill;
        scoreValue.text = score.ToString();
    }

    // When player health changed: update the displayed health bar
    private void OnHealthChanged(PlayerHealthChangedEvent phce)
    {
        healthBar.fillAmount = phce.newHealth;
        healthBar.color = Color.Lerp(Color.red, Color.green, phce.newHealth);
    }

    // Activate double points effect on pickup
    private void OnDoublePoints(DoublePointsEvent dpe)
    {
        StartCoroutine(ActivateDoublePoints(doublePointTime));
    }

    private void OnPopupOpened(PopupOpenedEvent poe)
    {
        NumPopupsOpen++;
    }

    private void OnPopupClosed(PopupClosedEvent pce)
    {
        NumPopupsOpen--;
    }

    private void OnPlayerDead(PlayerDeadEvent pde)
    {
        PauseGame();
        gameOverScreen.Open();
    }

    private void OnRestartGame(GameRestartEvent gre)
    {
        ResumeGame();
        SceneManager.LoadScene(0);
    }

    private void OnGameWon(GameWonEvent gwe)
    {
        PauseGame();
        victoryScreen.Open(score);
    }

    private void OnWaveStart(WaveStartedEvent wse)
    {
        waveValue.text = wse.currWave.ToString() + "/" + SceneController.NUM_WAVES.ToString();
    }

    // Update displayed grenade count if changed
    private void OnGrenadeCountChanged(GrenadeCountChangedEvent gcce)
    {
        switch (gcce.newGrenadeCount)
        {
            case 0:
                grenadeIndicator1.color = usedGrenade;
                grenadeIndicator2.color = usedGrenade;
                grenadeIndicator3.color = usedGrenade;
                break;
            case 1:
                grenadeIndicator1.color = availableGrenade;
                grenadeIndicator2.color = usedGrenade;
                grenadeIndicator3.color = usedGrenade;
                break;
            case 2:
                grenadeIndicator1.color = availableGrenade;
                grenadeIndicator2.color = availableGrenade;
                grenadeIndicator3.color = usedGrenade;
                break;
            default:
                grenadeIndicator1.color = availableGrenade;
                grenadeIndicator2.color = availableGrenade;
                grenadeIndicator3.color = availableGrenade;
                break;
        }
    }

    private void OnEnable()
    {
        PopupOpenedEvent.Register(OnPopupOpened);
        PopupClosedEvent.Register(OnPopupClosed);
        DoublePointsEvent.Register(OnDoublePoints);
        GrenadeCountChangedEvent.Register(OnGrenadeCountChanged);
        WaveStartedEvent.Register(OnWaveStart);
        EnemyDeadEvent.Register(OnEnemyDead);
        PlayerHealthChangedEvent.Register(OnHealthChanged);
        PlayerDeadEvent.Register(OnPlayerDead);
        GameRestartEvent.Register(OnRestartGame);
        GameWonEvent.Register(OnGameWon);
    }

    // Start is called before the first frame update
    void Start()
    {
        // init score
        score = 0;
        scoreValue.text = score.ToString();

        // init wave counter
        waveValue.text = "0/5";

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
        NumPopupsOpen = 0;

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
        if (Input.GetKeyDown(KeyCode.Escape) && NumPopupsOpen == 0)
        {
            PauseGame();
            optionsPopup.Open();
        }
    }

    private void OnDisable()
    {
        PopupOpenedEvent.Unregister(OnPopupOpened);
        PopupClosedEvent.Unregister(OnPopupClosed);
        DoublePointsEvent.Unregister(OnDoublePoints);
        GrenadeCountChangedEvent.Unregister(OnGrenadeCountChanged);
        WaveStartedEvent.Unregister(OnWaveStart);
        EnemyDeadEvent.Unregister(OnEnemyDead);
        PlayerHealthChangedEvent.Unregister(OnHealthChanged);
        PlayerDeadEvent.Unregister(OnPlayerDead);
        GameRestartEvent.Unregister(OnRestartGame);
        GameWonEvent.Unregister(OnGameWon);
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
