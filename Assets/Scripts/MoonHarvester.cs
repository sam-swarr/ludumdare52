using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonHarvester : MonoBehaviour
{
    [Tooltip("Harvester horizontal force.")]
    [SerializeField]
    private float HorizontalForce;

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
        MoonHarvesterRB = this.GetComponent<Rigidbody2D>();
        MoonHarvesterCollider = this.GetComponent<Collider2D>();
        PreviousPosition = transform.position;
    }

    void FixedUpdate()
    {
        /**
         *   Check to see if harvester is stuck and should reverse directions.
         */
        if (PreviousPosition == transform.position)
        {
            NumFramesStationary++;
            // Reverse direction if we haven't moved in the last several frames.
            if (NumFramesStationary >= StationFrameThreshold)
            {
                HorizontalForce *= -1;
                // Give the harvester a little push so that it won't get stuck under platforms in some situations.
                MoonHarvesterRB.AddForce(new Vector2(HorizontalForce * ReverseDirectionImpulseStrength, 0f), ForceMode2D.Impulse);
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
            MoonHarvesterRB.AddForce(new Vector2(HorizontalForce, 0f));

            // limit harvester speed while on ground
            if (MoonHarvesterRB.velocity.magnitude >= MaxSpeed)
            {
                MoonHarvesterRB.velocity = MoonHarvesterRB.velocity.normalized * MaxSpeed;
            }
        }
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // collision.otherCollider.IsTouchingLayers(Lay)
    //    //print(collision.otherCollider.name + " hit " + collision.collider.name);
    //    //Vector3 angleOfWall = collision.collider.gameObject.transform.rotation.eulerAngles;
    //    //print(angleOfWall);
    //    //foreach (ContactPoint2D contact in collision.contacts)
    //    //{
    //    //    //print(contact.collider.name + " hit " + contact.otherCollider.name);
    //    //    //// Visualize the contact point
    //    //    //Debug.DrawRay(contact.point, contact.normal, Color.white);
    //    //}
    //}
}
