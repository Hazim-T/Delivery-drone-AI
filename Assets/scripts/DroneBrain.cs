using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.Mathematics;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class DroneBrain : Agent
{
    public Transform DropZone;

    public Transform[] spawnPoints;

    //private float thrust = 0.0368f; // depends on the speed

    //private Vector3[] points = {new Vector3()};
    
    private float MAX_DISTANCE = 80f;


    public override void OnEpisodeBegin()
    {
        //reset drone position and rotation
        transform.localPosition = new Vector3(-11.7f, 13f, 2f);
        transform.localRotation = new Quaternion(0, 0, 0, 0);

        //spawn DropZone on one of the predetermined positions.
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform randomSpawnPoint = spawnPoints[randomIndex];
        DropZone.localPosition = randomSpawnPoint.localPosition;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition - DropZone.localPosition);
        //sensor.AddObservation(Vector3.Distance(transform.localPosition, DropZone.localPosition));

        AddReward(-0.01f); // time penalty to motivate faster runs
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float speed = 200f;
        float movex = actions.ContinuousActions[0];
        float movey = actions.ContinuousActions[1] / 2;
        float movez = actions.ContinuousActions[2];

        // Counteract gravity by setting the Rigidbody's velocity
        Vector3 velocity = new Vector3(movex, movey , movez) * speed * Time.fixedDeltaTime;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = velocity;


        //distance to target
        /*
        if (Vector3.Distance(transform.localPosition, DropZone.localPosition) < 10f)
        {
            AddReward(0.003f);
        }

        else if (Vector3.Distance(transform.localPosition, DropZone.localPosition) < 3f)
        {
            AddReward(0.01f);
        }
         
        
        if (Vector3.Distance(transform.localPosition, DropZone.localPosition) < 5f)
        {
            AddReward(1/(Vector3.Distance(transform.localPosition, DropZone.localPosition) + 0.5f)); // [0, 100]
        }
        */
        float distance_scaled = Vector3.Distance(DropZone.localPosition, transform.localPosition) / MAX_DISTANCE; // [0, 1] approx
        AddReward(-distance_scaled / 10); // [0, 0.1] negative

        /*
        if(Vector3.Distance(transform.localPosition, DropZone.localPosition) < 3f)
        {
            AddReward(1 / (transform.localPosition.y + 1));
        }
        */
        //////math.clampf()
        // could add the conditional reward and leave this as well (no else)
    }
 
    public void OnCollisionEnter(Collision collision)
    {
            AddReward(-100f);
            EndEpisode();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Border")){
            AddReward(-100f);
            EndEpisode();
        }

        if (other.CompareTag("DropZone")) 
        {
            AddReward(100f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousactions = actionsOut.ContinuousActions;

        continuousactions[0] = 0;
        continuousactions[1] = 0;
        continuousactions[2] = 0;

        continuousactions[0] = Input.GetAxisRaw("Horizontal");        

        if (Input.GetKey("i")){
            continuousactions[1] = 1;
        }

        if (Input.GetKey("k")) {
            continuousactions[1] = -1;
        }

        continuousactions[2] = Input.GetAxisRaw("Vertical");
    }
}