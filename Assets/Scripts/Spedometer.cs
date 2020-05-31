using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spedometer : MonoBehaviour
{
    public Rigidbody rb;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = (Mathf.Round(rb.velocity.magnitude * 3.6f)).ToString();
    }
}
