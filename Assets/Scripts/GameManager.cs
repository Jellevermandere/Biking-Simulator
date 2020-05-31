using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static int playerPlace;
    public float StartDelay;
    public float countdownTimer;
    private GameObject player;

    public int nrOfLaps = 1;
    public Text endtext;
    private bool isPlaying;

    public List <GameObject> players = new List<GameObject>();


    private int PlayerPlace;

    // Start is called before the first frame update
    void Start()
    {
        if (endtext!= null)
        {
            endtext.text = "You finished as nr: " + playerPlace + "!";
        }
        Time.timeScale = 1;
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            isPlaying = true;
            players.AddRange(GameObject.FindGameObjectsWithTag("NPC"));
            player = GameObject.FindGameObjectWithTag("Player");
            players.Add(player);
        }
        

        Debug.Log(players.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if(players.Count > 0 && isPlaying)
        {
            players.Sort(SortByScore);
            playerPlace = players.IndexOf(player) + 1;
        }
        
        //Debug.Log("1st: " + players[0] + ", 2nd: " + players[1] + ", 3rd: " + players[2]);
    }

    // C#
    static int SortByScore(GameObject p1, GameObject p2)
    {
        return p2.GetComponent<PosAlongTrack>().currentPosAlongTrack.CompareTo(p1.GetComponent<PosAlongTrack>().currentPosAlongTrack);

    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void UnPauseGame()
    {
        Time.timeScale = 1f;
    }
    public void EndGame()
    {
        isPlaying = false;
        Invoke("EndScreen", 3f);
    }

    public void EndScreen()
    {
        SceneManager.LoadScene(2);
    }
}
