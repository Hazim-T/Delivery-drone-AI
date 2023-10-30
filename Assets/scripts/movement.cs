using UnityEngine;
using System.Collections.Generic;

namespace DronesAttack
{
    public class PlayerMovementController : MonoBehaviour
    {
        public float ForwardMovementSpeed = 0.25f;
        public float SideMovementSpeed = 0.1f;
        public float VerticalMovementSpeed = 0.125f;
        public float rotationSpeed = 45.0f;
        public Rigidbody rb;
        public float force = 5.0f;
        private Quaternion originalRotation;
        public float bounceForce = 10.0f;



        private void Start()
        {
            rb.mass = 1.0f; // Set the mass to 1 kilogram
            rb.drag = 0.5f; // Set the drag to 0.5
            originalRotation = transform.rotation;
        }


        private Dictionary<string, KeyCode> movementKeyBindings = new Dictionary<string, KeyCode>()
        {
            { "FORWARD", KeyCode.W },
            { "BACKWARD", KeyCode.S },
            { "LEFT", KeyCode.A },
            { "RIGHT", KeyCode.D },
            { "UP", KeyCode.Space },
            { "DOWN", KeyCode.LeftShift },
            {"E", KeyCode.E },
            {"Q", KeyCode.Q }
        };

        public void FixedUpdate()
        {

            if (Input.GetKey(this.movementKeyBindings["FORWARD"]))
            {
                this.transform.position += new Vector3(
                    this.transform.forward.x * this.ForwardMovementSpeed,
                    0,
                    this.transform.forward.z * this.ForwardMovementSpeed
                );
                rb.AddForce(Vector3.forward * force);
            }

            if (Input.GetKey(this.movementKeyBindings["BACKWARD"]))
            {
                this.transform.position += new Vector3(
                    this.transform.forward.x * (-this.ForwardMovementSpeed / 1.95f),
                    0,
                    this.transform.forward.z * (-this.ForwardMovementSpeed / 1.95f)
                );
                rb.AddForce(Vector3.forward * -force);
            }

            if (Input.GetKey(this.movementKeyBindings["LEFT"]))
            {
                this.transform.Translate(Vector3.left * this.SideMovementSpeed);
                rb.AddForce(Vector3.left * force);
            }

            if (Input.GetKey(this.movementKeyBindings["RIGHT"]))
            {
                this.transform.Translate(Vector3.right * this.SideMovementSpeed);
                rb.AddForce(Vector3.right * force);
            }

            if (Input.GetKey(this.movementKeyBindings["UP"]))
            {
                this.transform.Translate(Vector3.up * this.VerticalMovementSpeed);
                rb.AddForce(Vector3.up * force);
            }

            if (Input.GetKey(this.movementKeyBindings["DOWN"]))
            {
                this.transform.Translate(Vector3.down * this.VerticalMovementSpeed);
                rb.AddForce(Vector3.down * force);
            }
            if (Input.GetKey(this.movementKeyBindings["E"]))
            {
                this.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(this.movementKeyBindings["Q"]))
            {
                this.transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * rotationSpeed);
        }
        void OnCollisionEnter(Collision collision)
        {
            // Calculate the opposite direction of the collision normal
            Vector3 bounceDirection = -collision.contacts[0].normal;

            // Apply a force in the opposite direction
            rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
        }
    }
}