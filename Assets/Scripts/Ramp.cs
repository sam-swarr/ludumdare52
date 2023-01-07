using UnityEngine;

public class Ramp : MonoBehaviour
{
    public enum RampType
    {
        LMB,
        RMB,
    }
    public void SetRampType(RampType type)
    {
        Color c;
        if (type == RampType.LMB)
        {
            c = Color.red;
        }
        else
        {
            c = Color.blue;
        }
        this.GetComponent<SpriteRenderer>().color = c;
    }
}
