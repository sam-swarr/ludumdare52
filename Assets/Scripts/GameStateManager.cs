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

    [Tooltip("GameObject representing moon harvester graphic.")]
    [SerializeField]
    private GameObject MoonHarvesterGraphic;

    [Tooltip("GameObject representing actual moon harvester.")]
    [SerializeField]
    private GameObject MoonHarvester;

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
        }
    }

    public void StartGame()
    {
        TitleGraphic.SetActive(false);
        HeliumText.gameObject.SetActive(true);
        CanistersText.gameObject.SetActive(true);
        MoonHarvesterGraphic.SetActive(false);
        MoonHarvester.SetActive(true);
        State = GameState.Playing;
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
