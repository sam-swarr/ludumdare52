using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance = null;

    public enum GameState
    {
        TitleScreen,
        Playing,
        GameOver,
    }

    [Tooltip("UI Text object for rendering the helium value.")]
    [SerializeField]
    private TMPro.TMP_Text HeliumText;

    [Tooltip("UI Text object for rendering the number of canisters harvested.")]
    [SerializeField]
    private TMPro.TMP_Text CanistersText;

    [Tooltip("GameObject representing title graphic.")]
    [SerializeField]
    private GameObject TitleGraphic;

    [Tooltip("GameObject representing game over dialog.")]
    [SerializeField]
    private GameObject GameOverDialog;

    [Tooltip("GameObject representing actual moon harvester.")]
    [SerializeField]
    private MoonHarvester MoonHarvester;

    [Tooltip("GameObject representing RampSpawner.")]
    [SerializeField]
    private RampSpawner RampSpawner;

    [Tooltip("GameObject representing LevelSpawner.")]
    [SerializeField]
    private LevelSpawner LevelSpawner;

    [Tooltip("GameObject representing earth.")]
    [SerializeField]
    private Earth Earth;

    [Tooltip("Helium depletion rate (how many fixed frames per unit depleted)")]
    [SerializeField]
    private int HeliumDepletionRate = 5;
    private int Counter = 0;

    [Tooltip("Helium canister value")]
    [SerializeField]
    private int HeliumCanisterValue = 100;

    private int HeliumAmount = 1000;
    private int CanistersHarvested = 0;

    public GameState State = GameState.TitleScreen;

    public virtual void Awake()
    {
        GameStateManager.Instance = this;
    }

    void FixedUpdate()
    {
        if (Instance.State != GameState.Playing)
        {
            return;
        }

        Counter++;
        if (Counter >= HeliumDepletionRate)
        {
            HeliumAmount--;
            HeliumText.text = "Fuel: " + HeliumAmount.ToString();
            Counter = 0;
            if (HeliumAmount <= 0)
            {
                GameOver();
            }
        }
    }

    public void StartGame()
    {
        TitleGraphic.SetActive(false);

        HeliumAmount = 1000;
        HeliumText.text = HeliumAmount.ToString();
        HeliumText.gameObject.SetActive(true);

        CanistersHarvested = 0;
        CanistersText.text = "Canisters: " + CanistersHarvested.ToString();
        CanistersText.gameObject.SetActive(true);

        RampSpawner.Reset();
        LevelSpawner.Reset();
        MoonHarvester.TurnOn();

        State = GameState.Playing;
    }

    public void ShowTitleScreen()
    {
        State = GameState.TitleScreen;
        GameOverDialog.SetActive(false);
        TitleGraphic.SetActive(true);
        HeliumText.gameObject.SetActive(false);
        CanistersText.gameObject.SetActive(false);
        MoonHarvester.ResetPosition();
        Earth.Reset();
        RampSpawner.Reset();
        LevelSpawner.Reset();
    }

    public void GameOver()
    {
        State = GameState.GameOver;
        GameOverDialog.SetActive(true);
        MoonHarvester.TurnOff();
    }

    public void HeliumCollected()
    {
        HeliumAmount += HeliumCanisterValue;
        HeliumText.text = HeliumAmount.ToString();

        CanistersHarvested++;
        CanistersText.text = "Canisters: " + CanistersHarvested.ToString();
    }

    public void TrapHit()
    {
        print("TRAP HIT");
    }

    public void RampBuilt(int heliumCost)
    {
        print("RAMP BUILT: " + heliumCost);
        HeliumAmount -= heliumCost;
        HeliumText.text = HeliumAmount.ToString();
    }
}
