using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosAlongTrack : MonoBehaviour
{
    public bool isPlayer;
    public float currentPosAlongTrack = 0f;
    private Vector3 lastPosition;
    private TerrainGeneration terrain;
    private GameManager gm;
    public Animator anim;
    public IKControl ikControl;
    private int nrLap;

    private bool hasPassedMidPoint;
    public int currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        terrain = GameObject.FindGameObjectWithTag("Terrain").GetComponent<TerrainGeneration>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        increasePointAlongTrack();
        currentPosition = gm.players.IndexOf(gameObject) + 1;
    }

    void increasePointAlongTrack()
    {
        currentPosAlongTrack += Vector3.SignedAngle(transform.position - terrain.worldOffset, lastPosition - terrain.worldOffset, Vector3.up);
        lastPosition = transform.position;
    }

    void Finish()
    {
        Debug.Log("finished at :" + currentPosition);

        if (isPlayer)
        {
            gm.EndGame();
            ikControl.ikActive = false;
            anim.SetTrigger("Finish");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "HalfWayPoint")
        {
            hasPassedMidPoint = true;
        }

        if (other.tag == "FinishLine" && hasPassedMidPoint)
        {
            hasPassedMidPoint = false;
            nrLap++;
            if(nrLap > gm.nrOfLaps)
            {
                Finish();
            }

        }
    }
}
