using UnityEngine;

public class Trap : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent<MoonHarvester>(out var _))
        {
            GameStateManager.Instance.TrapHit();
            Destroy(this.gameObject);
        }
    }
}
