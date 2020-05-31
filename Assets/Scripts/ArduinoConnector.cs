using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.IO.Ports; // this enables the IO port namespace

// ************* This script manages the Arduino Communication ******************* //
// arduino script at the bottom //

public class ArduinoConnector : MonoBehaviour
{
    [Header("ConnectionSettings")]
    public bool useArduino;
    public string IOPort = "/dev/cu.HC05-SPPDev";  // /dev/cu.usbmodem1414401  // Change this to whatever port your Arduino is connected to, this is the port for the specefic bluetooth adaptor used (HC-05 Wireless Bluetooth RF Transceiver)
    public int baudeRate = 9600; //this must match the bauderate of the Arduino script
    [HideInInspector]
    public Vector2 inputValues = Vector2.zero;
    [Range(-1,1)]
    public int SteeringDirection;
    public float angleConversionFactor;
    public float WheelRaduis;

    /*
    [HideInInspector]
	public SerialPort sp;
    */
    private string recievedValue;

    private bool hasPassed;
    private float timeBtwPassings;

    /*
    // Start is called before the first frame update
    void Start()
    {
        if (useArduino) ActivateSP();
    }

    // Update is called once per frame
    void Update()
    {

        if (useArduino && sp.IsOpen)
        {
            try
            {
                recievedValue = sp.ReadLine(); //reads the serial input
                //Debug.Log(recievedValue);
                SetDirection(recievedValue); //translates the string into a Vector
            }
            catch (System.Exception)
            {

            }
        }

    }
    
    void ActivateSP()
    {
        sp = new SerialPort(IOPort, baudeRate, Parity.None, 8, StopBits.One);

        sp.Open();
        sp.ReadTimeout = 35;
    }

    */
    void SetDirection(string message)
    {
        // InputStringFormat:  "potentiometerValue between 0 & 1023,magnet passed 1 (no) 0 (yes)"
        char seperator = ',';
        string[] values = message.Split(seperator);

        //convert the input to an angle between -1 & 1
        
        if (float.Parse(values[0]) < 256)
        {
            inputValues.x = (256) / angleConversionFactor * SteeringDirection;
        }
        else if(float.Parse(values[0]) > 768)
        {
            inputValues.x = (768) / angleConversionFactor * SteeringDirection;
        }
        else
        {
            inputValues.x = (float.Parse(values[0]) - 512) / angleConversionFactor * SteeringDirection;
        }

        //set the speedvalue
        float input = float.Parse(values[1]);

        if (input == 0 && hasPassed == false)
        {
            hasPassed = true;
            inputValues.y = WheelRaduis * 2 * Mathf.PI / timeBtwPassings;
            timeBtwPassings = 0f;
        }
        else if (input == 1)
        {
            hasPassed = false;
        }

        if(timeBtwPassings > 5)
        {
            inputValues.y = 0f;
        }
        timeBtwPassings += Time.deltaTime;

        Debug.Log(inputValues);

    }

}

/* Put this code on your arduino 

    // MPU-6050 Gyroscope & accelerometer read-out to Unity
    // By Arduino User JohnChi // modified by Jelle Vermandere
    // March 21, 2020
    // Public Domain
    #include<Wire.h>
    const int MPU=0x68; 
    int16_t AcX,AcY,AcZ,Tmp,GyX,GyY,GyZ;

    void setup(){
      Wire.begin();
      Wire.beginTransmission(MPU);
      Wire.write(0x6B); 
      Wire.write(0);    
      Wire.endTransmission(true);
      Serial.begin(9600);
    }
    void loop(){
      Wire.beginTransmission(MPU);
      Wire.write(0x3B);  
      Wire.endTransmission(false);
      Wire.requestFrom(MPU,12,true);  
      AcX=Wire.read()<<8|Wire.read();    
      AcY=Wire.read()<<8|Wire.read();  
      AcZ=Wire.read()<<8|Wire.read();  
      GyX=Wire.read()<<8|Wire.read();  
      GyY=Wire.read()<<8|Wire.read();  
      GyZ=Wire.read()<<8|Wire.read();  
  
      Serial.print(AcX); Serial.print(",");
      Serial.print(AcY); Serial.print(",");
      Serial.print(AcZ); Serial.print(",");

      Serial.print(GyX); Serial.print(",");
      Serial.print(GyY); Serial.print(",");
      Serial.print(GyZ); 
      Serial.println("");
      Serial.flush();
      delay(25);
    }
*/
