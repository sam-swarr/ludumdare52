using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [Tooltip("UI Text object for rendering the helium value.")]
    [SerializeField]
    private UnityEngine.UI.Text HeliumText;

    private int HeliumAmount = 1000;

    void FixedUpdate()
    {
        HeliumAmount--;
        HeliumText.text = HeliumAmount.ToString();
    }
}
