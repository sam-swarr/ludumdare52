using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RampSpawner : MonoBehaviour
{
    [Tooltip("Ramp Prefab")]
    [SerializeField]
    private GameObject RampPrefab;

    [Tooltip("Amount of helium per unit in length that ramps cost to build.")]
    [SerializeField]
    private float RampHeliumCost = 50f;

    [SerializeField]
    private bool IsLMBDown;
    [SerializeField]
    private Vector2 LMB_Down_Position;
    [SerializeField]
    private Vector2 LMB_Up_Position;
    private GameObject LMB_Ramp;

    [SerializeField]
    private bool IsRMBDown;
    [SerializeField]
    private Vector2 RMB_Down_Position;
    [SerializeField]
    private Vector2 RMB_Up_Position;
    private GameObject RMB_Ramp;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsRMBDown)
        {
            IsLMBDown = true;
            LMB_Down_Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0) && !IsRMBDown)
        {
            IsLMBDown = false;
            LMB_Up_Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject newRamp = SpawnRamp(LMB_Down_Position, LMB_Up_Position, Ramp.RampType.LMB);
            if (newRamp != null)
            {
                if (LMB_Ramp != null)
                {
                    Destroy(LMB_Ramp);
                }
                LMB_Ramp = newRamp;
            }
        }
        else if (Input.GetMouseButtonDown(1) && !IsLMBDown)
        {
            IsRMBDown = true;
            RMB_Down_Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(1) && !IsLMBDown) {
            IsRMBDown = false;
            RMB_Up_Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject newRamp = SpawnRamp(RMB_Down_Position, RMB_Up_Position, Ramp.RampType.RMB);
            if (newRamp != null)
            {
                if (RMB_Ramp != null)
                {
                    Destroy(RMB_Ramp);
                }
                RMB_Ramp = newRamp;
            }
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

    private GameObject SpawnRamp(Vector2 endpointOne, Vector2 endpointTwo, Ramp.RampType rampType)
    {
        // Early exit and don't spawn anything if ramp would intersect with harvester
        RaycastHit2D harvesterHit = Physics2D.Linecast(endpointOne, endpointTwo, LayerMask.GetMask("Harvester"));
        if (harvesterHit.collider != null)
        {
            return null;
        }

        Vector3 midpoint = new(
            endpointOne.x + (endpointTwo.x - endpointOne.x) / 2.0f,
            endpointOne.y + (endpointTwo.y - endpointOne.y) / 2.0f,
            0f);

        float rampLength = Vector3.Distance(endpointOne, endpointTwo);

        float rotationAngle = Mathf.Atan2(endpointTwo.y - midpoint.y, endpointTwo.x - midpoint.x) * Mathf.Rad2Deg;
        GameObject newRamp = Instantiate(RampPrefab, midpoint, Quaternion.identity);
        newRamp.transform.Rotate(0, 0, rotationAngle);
        newRamp.transform.localScale = new Vector3(
            rampLength,
            newRamp.transform.localScale.y,
            newRamp.transform.localScale.z);
        newRamp.GetComponent<Ramp>().SetRampType(rampType);

        GameStateManager.Instance.RampBuilt((int)(rampLength * RampHeliumCost));

        return newRamp;
    }
}
