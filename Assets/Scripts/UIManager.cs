using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager gm;

    public RectTransform[] playersUI;
    public GameObject[] players;

    public float uiOffset = 60;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playersUI.Length; i++)
        {
            playersUI[i].anchoredPosition = new Vector3(40, gm.players.IndexOf(players[i]) * (- uiOffset),0);
        }
    }
}
