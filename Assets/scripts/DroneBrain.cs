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

    public Transform DropZone;

    private int NumOfCollision = 0;

    public Transform[] spawnPoints;

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
        sensor.AddObservation(Vector3.Distance(transform.localPosition, DropZone.localPosition));

        AddReward(-0.01f); // time penalty to motivate faster runs
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float speed = 2f;   
        //<ActionBuffers> SET THIS SET THIS
        float movex = actions.ContinuousActions[0];
        float movey = actions.ContinuousActions[1]/4;
        float movez = actions.ContinuousActions[2];
        
        transform.Translate(new Vector3(movex, movey, movez) * speed * Time.fixedDeltaTime);


        //distance to target
        if (Vector3.Distance(transform.localPosition, DropZone.localPosition) > 5f)
        {
            AddReward((1f / (Vector3.Distance(transform.localPosition, DropZone.localPosition) + 1f)) / 5 );
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        //transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, 0, 0, 0), Time.deltaTime * 45f);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        NumOfCollision++;

        if (NumOfCollision == 5)
        {
            AddReward(-100f);
            EndEpisode();
        }

          //if (collision.collider.tag == "DropZone")
          if (collision.collider.CompareTag("DropZone"))
        {
            AddReward(150f);
            EndEpisode();
        }
        if (collision.collider.CompareTag("Border")) {
            AddReward(-100f);
            EndEpisode();
        }
        else
        {
            AddReward(-20f);
        }

    }

    //public override void Heuristic() is for debugging. In other words you control the drone yourself to test how
    //good the AI is.

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousactions = actionsOut.ContinuousActions;
        continuousactions[0] = Input.GetAxisRaw("Horizontal");
        continuousactions[1] = 0; //idk
        continuousactions[2] = Input.GetAxisRaw("Vertical");
    }

}