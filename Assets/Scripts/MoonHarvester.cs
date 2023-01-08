using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonHarvester : MonoBehaviour
{
    public static MoonHarvester Instance = null;

    [Tooltip("GameObject representing top center point of harvester")]
    [SerializeField]
    private GameObject TopPoint;

    [Tooltip("GameObject with forward sprite animation")]
    [SerializeField]
    private GameObject HarvesterForward;

    [Tooltip("GameObject with reverse sprite animation")]
    [SerializeField]
    private GameObject HarvesterReverse;

    [Tooltip("GameObject with static sprite")]
    [SerializeField]
    private GameObject HarvesterStatic;

    [Tooltip("Spawn position")]
    [SerializeField]
    private GameObject SpawnPosition;

    [Tooltip("Harvester horizontal force magnitude.")]
    [SerializeField]
    private float HorizontalForce;
    private int HorizontalForceDirection = 1;

    [Tooltip("Harvester max speed.")]
    [SerializeField]
    private float MaxSpeed;

    [Tooltip("Reverse direction threshold (in frames).")]
    [SerializeField]
    private int StationFrameThreshold;

    [Tooltip("Reverse direction impulse strength.")]
    [SerializeField]
    private float ReverseDirectionImpulseStrength;

    private Rigidbody2D MoonHarvesterRB;
    private Collider2D MoonHarvesterCollider;
    private Vector3 PreviousPosition;
    [SerializeField]
    private int NumFramesStationary;
    [SerializeField]
    private bool InContactWithGround = false;

    // Start is called before the first frame update
    void Start()
    {
        MoonHarvester.Instance = this;
        ResetPosition();
        MoonHarvesterRB = this.GetComponent<Rigidbody2D>();
        MoonHarvesterCollider = this.GetComponent<Collider2D>();
        PreviousPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (GameStateManager.Instance.State != GameStateManager.GameState.Playing)
        {
            return;
        }

        /**
         *   Check to see if harvester is stuck and should reverse directions.
         */
        if (PreviousPosition == transform.position)
        {
            NumFramesStationary++;
            // Reverse direction if we haven't moved in the last several frames.
            if (NumFramesStationary >= StationFrameThreshold)
            {
                ReverseDirection();
                NumFramesStationary = 0;
            }
        }
        else
        {
            NumFramesStationary = 0;
        }
        PreviousPosition = transform.position;

        /**
         *  Apply force to move harvester.
         */
        InContactWithGround = MoonHarvesterCollider.IsTouchingLayers(LayerMask.GetMask("Platforms"));
        if (InContactWithGround)
        {
            MoonHarvesterRB.AddForce(new Vector2(HorizontalForce * HorizontalForceDirection, 0f));

            // limit harvester speed while on ground
            if (MoonHarvesterRB.velocity.magnitude >= MaxSpeed)
            {
                MoonHarvesterRB.velocity = MoonHarvesterRB.velocity.normalized * MaxSpeed;
            }
        }
    }
    public void ResetPosition()
    {
        HorizontalForceDirection = 1;
        transform.SetPositionAndRotation(
            SpawnPosition.transform.position,
            SpawnPosition.transform.rotation);
        HarvesterForward.SetActive(false);
        HarvesterReverse.SetActive(false);
        HarvesterStatic.SetActive(true);
    }

    public void TurnOn()
    {
        HarvesterForward.SetActive(true);
        HarvesterReverse.SetActive(false);
        HarvesterStatic.SetActive(false);
    }

    public void TurnOff()
    {
        HarvesterForward.SetActive(false);
        HarvesterReverse.SetActive(false);
        HarvesterStatic.SetActive(true);
    }
    private void ReverseDirection()
    {
        HorizontalForceDirection *= -1;
        // Give the harvester a little push so that it won't get stuck under platforms in some situations.
        MoonHarvesterRB.AddForce(new Vector2(HorizontalForce * HorizontalForceDirection * ReverseDirectionImpulseStrength, 0f), ForceMode2D.Impulse);
        HarvesterForward.SetActive(HorizontalForceDirection > 0);
        HarvesterReverse.SetActive(HorizontalForceDirection <= 0);
    }
}
