using UnityEngine;

public class Ramp : MonoBehaviour
{
    public enum RampType
    {
        LMB,
        RMB,
        Static,
    }
    public void SetRampType(RampType type)
    {
        Color c;
        if (type == RampType.LMB)
        {
            c = Color.red;
        }
        else if (type == RampType.RMB)
        {
            c = Color.blue;
        } else
        {
            c = Color.gray;
        }
        this.GetComponent<SpriteRenderer>().color = c;
    }
}
