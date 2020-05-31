using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BikeController))]
public class PlayerController : MonoBehaviour
{
    BikeController bikeController;
    public ArduinoConnector arduinoConnector;
    

    public bool useKeyboard;
    public float keyBoardMaxSpeed;

    

    private void Start()
    {
        bikeController = GetComponent<BikeController>();

        if(!arduinoConnector.useArduino && !useKeyboard)
        {
            bikeController.setRotationAndSpeed(new Vector2(0, keyBoardMaxSpeed));
        }
    }

    private void Update()
    {
        if (arduinoConnector.useArduino)
        {
            bikeController.setRotationAndSpeed(new Vector2( arduinoConnector.inputValues.x, keyBoardMaxSpeed));
        }
        else if (useKeyboard)
        {
            bikeController.setRotationAndSpeed(new Vector2( Input.GetAxis("Horizontal")/3f, Input.GetAxis("Vertical") * keyBoardMaxSpeed));
        }
        

        
    }
    public void setRotation(float value)
    {
        bikeController.setRotationAndSpeed(new Vector2(value,keyBoardMaxSpeed));

    }


}
