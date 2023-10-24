using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionHandler : MonoBehaviour
{
    // This function is called when the GameObject collides with another GameObject
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("delivery spot"))
        {
            Debug.Log("Parcel delivered");
            // You can perform other actions here as needed.
        }
    }
}