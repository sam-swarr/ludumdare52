using UnityEngine;

public class HeliumCanister : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent<MoonHarvester>(out var _))
        {
            GameStateManager.Instance.HeliumCollected();
            Destroy(this.gameObject);
        }
    }
}
