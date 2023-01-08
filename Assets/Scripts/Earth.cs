using UnityEngine;

public class Earth : MonoBehaviour
{
    [Tooltip("Earth speed")]
    [SerializeField]
    private float SpeedX = 0.01f;

    [Tooltip("Earth speed")]
    [SerializeField]
    private float SpeedY = 0.005f;

    void FixedUpdate()
    {
        if (GameStateManager.Instance.State != GameStateManager.GameState.Playing)
        {
            return;
        }

        transform.position = new Vector3(
            transform.position.x - SpeedX,
            transform.position.y + SpeedY,
            transform.position.z);
    }
}
