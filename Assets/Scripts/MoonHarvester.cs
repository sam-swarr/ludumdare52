using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonHarvester : MonoBehaviour
{
    [Tooltip("Harvester speed.")]
    [SerializeField]
    private float Speed = 1.0f;

    [Tooltip("Reverse direction threshold (in frames).")]
    [SerializeField]
    private int STATIONARY_FRAME_THRESHOLD = 12;

    private Rigidbody2D MoonHarvesterRB;
    private Vector3 PreviousPosition;
    private int NumFramesStationary = 0;

    // Start is called before the first frame update
    void Start()
    {
        MoonHarvesterRB = this.GetComponent<Rigidbody2D>();
        PreviousPosition = transform.position;
    }

    void FixedUpdate()
    {
        
        // TODO: only move harvester if it's touching the ground
        MoonHarvesterRB.velocity = new Vector2(Speed, MoonHarvesterRB.velocity.y);



    }

    private void LateUpdate()
    {
        if (PreviousPosition == transform.position)
        {
            NumFramesStationary++;
            // Reverse direction if we haven't moved in the last several frames.
            if (NumFramesStationary >= STATIONARY_FRAME_THRESHOLD)
            {
                Speed *= -1;
                NumFramesStationary = 0;
            }
        } else
        {
            NumFramesStationary = 0;
        }
        PreviousPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //print(collision.otherCollider.name + " hit " + collision.collider.name);
        //Vector3 angleOfWall = collision.collider.gameObject.transform.rotation.eulerAngles;
        //print(angleOfWall);
        //foreach (ContactPoint2D contact in collision.contacts)
        //{
        //    //print(contact.collider.name + " hit " + contact.otherCollider.name);
        //    //// Visualize the contact point
        //    //Debug.DrawRay(contact.point, contact.normal, Color.white);
        //}
    }
}
