using UnityEngine;

public class Earth : MonoBehaviour
{
    [Tooltip("Earth starting point.")]
    [SerializeField]
    private GameObject EarthSpawnPoint;

    [Tooltip("Earth speed")]
    [SerializeField]
    private float SpeedX = 0.01f;

    [Tooltip("Earth speed")]
    [SerializeField]
    private float SpeedY = 0.005f;

    void FixedUpdate()
    {
        if (GameStateManager.Instance.State == GameStateManager.GameState.TitleScreen)
        {
            return;
        }

        transform.position = new Vector3(
            transform.position.x - SpeedX,
            transform.position.y + SpeedY,
            transform.position.z);
    }

    public void Reset()
    {
        transform.position = EarthSpawnPoint.transform.position;
    }
}
