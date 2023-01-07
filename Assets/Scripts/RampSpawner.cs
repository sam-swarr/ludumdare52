using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RampSpawner : MonoBehaviour
{
    [Tooltip("Ramp Prefab")]
    [SerializeField]
    private GameObject RampPrefab;

    [SerializeField]
    private bool IsLMBDown;
    [SerializeField]
    private Vector2 LMB_Down_Position;
    [SerializeField]
    private Vector2 LMB_Up_Position;

    [SerializeField]
    private bool IsRMBDown;
    [SerializeField]
    private Vector2 RMB_Down_Position;
    [SerializeField]
    private Vector2 RMB_Up_Position;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsRMBDown)
        {
            IsLMBDown = true;
            LMB_Down_Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            IsLMBDown = false;
            LMB_Up_Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SpawnRamp(LMB_Down_Position, LMB_Up_Position);
        }
        else if (Input.GetMouseButtonDown(1) && !IsLMBDown)
        {
            IsRMBDown = true;
            RMB_Down_Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(1)) {
            IsRMBDown = false;
            RMB_Up_Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (IsLMBDown)
        {
            Debug.DrawLine(LMB_Down_Position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
        }

        if (IsRMBDown)
        {
            Debug.DrawLine(RMB_Down_Position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.blue);
        }
    }

    private void SpawnRamp(Vector2 endpointOne, Vector2 endpointTwo)
    {
        print("SPAWN");
        Vector3 midpoint = new Vector3(
            endpointOne.x + (endpointTwo.x - endpointOne.x) / 2.0f,
            endpointOne.y + (endpointTwo.y - endpointOne.y) / 2.0f,
            0f);

        Instantiate(RampPrefab, midpoint, Quaternion.identity);
    }
}
