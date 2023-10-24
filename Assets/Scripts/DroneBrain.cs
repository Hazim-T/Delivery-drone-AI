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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform ParcelTransform;

    private int NumOfCollision = 0;

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(0, 4, 1);
        ParcelTransform.position = new Vector3(Random.Range(0,1), Random.Range(3, 4), Random.Range(4, 5));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);  
        sensor.AddObservation(ParcelTransform.position);
        sensor.AddObservation(Vector3.Distance(ParcelTransform.localPosition, transform.localPosition));
        //drop zone position.
        //distance between drone and drop zone.
        //vector sensors around drone to avoid collision.
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float movex = actions.ContinuousActions[0];
        float movey = actions.ContinuousActions[1];
        float movez = actions.ContinuousActions[2];

        float speed = 10f;
        transform.Translate(new Vector3(movex, movey, movez) * speed * Time.fixedDeltaTime);
        AddReward(Vector3.Distance(ParcelTransform.localPosition, transform.localPosition) / 1000);
    }

    public void OnCollisionEnter(Collision collision)
    {
        NumOfCollision++;

        if (NumOfCollision == 5) {
            AddReward(-100f);
            EndEpisode();
        }

        if (collision.collider.tag == "parcel")
        {
            AddReward(100f);
            EndEpisode();
        }
        else {
            AddReward(-10f);
        }
        
    }

    //public override void Heuristic() is for debugging. In other words you control the drone yourself to test how
    //good the AI is.

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousactions = actionsOut.ContinuousActions;
        continuousactions[0] = Input.GetAxisRaw("Horizontal");
        continuousactions[1] = 0;
        continuousactions[2] = Input.GetAxisRaw("Vertical");
    }

}
