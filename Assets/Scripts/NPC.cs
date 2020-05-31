using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BikeController))]
public class NPC : MonoBehaviour
{
    //[Range (0,10)]
    public int pointSkips;
    public float maxSpeed;
    BikeController bikeController;
    TerrainGeneration terrain;
    public float minDistanceToPoint;

    public int nrOfCheckpoint;
    public int nrOfLaps = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        bikeController = GetComponent<BikeController>();
        terrain = GameObject.FindGameObjectWithTag("Terrain").GetComponent<TerrainGeneration>();
        
    }

    // Update is called once per frame
    void Update()
    {
        SetBikeValues();
        
        if(Vector3.Distance(transform.position, terrain.pointsOnCurve[nrOfCheckpoint]) < minDistanceToPoint)
        {
            nrOfCheckpoint += pointSkips;
            //Debug.Log(nrOfCheckpoint);

            if(nrOfCheckpoint >= terrain.pointsOnCurve.Length)
            {
                Debug.Log("finish");
                //nrOfLaps++;
                nrOfCheckpoint = 1;
            }
        }

        
    }


    void SetBikeValues()
    {
        Vector3 direction = transform.InverseTransformPoint(terrain.pointsOnCurve[nrOfCheckpoint]); //Vector3.Normalize( terrain.pointsOnCurve[nrOfCheckpoint] - transform.position);

        //transform.forward = terrain.pointsOnCurve[nrOfCheckpoint] - transform.position;
        bikeController.setRotationAndSpeed(new Vector2(direction.x/direction.magnitude, maxSpeed));
        //Debug.Log(direction.x);
        
    }
}
