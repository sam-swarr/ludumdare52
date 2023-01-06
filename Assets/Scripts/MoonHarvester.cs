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
    private int STATIONARY_FRAME_THRESHOLD;

    private Rigidbody2D MoonHarvesterRB;
    private Collider2D MoonHarvesterCollider;
    private Vector3 PreviousPosition;
    [SerializeField]
    private int NumFramesStationary;
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

    private void LateUpdate()
    {
        if (PreviousPosition == transform.position)
        {
            NumFramesStationary++;
            // Reverse direction if we haven't moved in the last several frames.
            if (NumFramesStationary >= STATIONARY_FRAME_THRESHOLD)
            {
                HorizontalForce *= -1;
                NumFramesStationary = 0;
            }
        } else
        {
            NumFramesStationary = 0;
        }
        PreviousPosition = transform.position;
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
