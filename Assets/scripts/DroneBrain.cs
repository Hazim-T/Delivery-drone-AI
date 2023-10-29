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

    public Transform DropZone;

    private int NumOfCollision = 0;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(-11.7f, 13f, 2f);
        // m_n_y TODO: you should make it reset to one of the 3 or 4 random positions you chose ----

        //DropZone.localPosition = new Vector3();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Distance(transform.localPosition, DropZone.localPosition));

        AddReward(-0.01f); // time penalty to motivate faster runs

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //<ActionBuffers> SET THIS SET THIS
        float movex = actions.ContinuousActions[0];
        float movey = actions.ContinuousActions[1];
        float movez = actions.ContinuousActions[2];

        float speed = 7f;
        transform.Translate(new Vector3(movex, movey, movez) * speed * Time.fixedDeltaTime);

        if (transform.position.y > 17f) {
            AddReward(-100f);
            //EndEpisode();
        }

        //distance from parcel
        {
            AddReward((1f / (Vector3.Distance(ParcelTransform.localPosition, transform.localPosition) + 1f))/10 );
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        NumOfCollision++;

        if (NumOfCollision == 5)
        {
            AddReward(-100f);
            EndEpisode();
        }

          //if (collision.collider.tag == "DropZone")
          if (collision.collider.CompareTag("DropZone"))
        {
            AddReward(100f);
            EndEpisode();
        }
        else
        {
            AddReward(-10f);
        }

    }

    //public override void Heuristic() is for debugging. In other words you control the drone yourself to test how
    //good the AI is.

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousactions = actionsOut.ContinuousActions;
        continuousactions[0] = Input.GetAxisRaw("Horizontal");
        continuousactions[1] = 0; //idk how to
        continuousactions[2] = Input.GetAxisRaw("Vertical");
    }

}
