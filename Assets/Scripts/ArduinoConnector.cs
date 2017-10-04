/* ArduinoConnector by Alan Zucconi
 * http://www.alanzucconi.com/?p=2979
 */
using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;

public class ArduinoConnector : MonoBehaviour {
    
    private SerialPort stream;
    public String datos;

    public void Start () {
        // Opens the serial port
        datos = "";
        stream = new SerialPort("COM4", 38400);
        stream.Open();
    }
    
    public void Update()
    {

        //Debug.Log(stream.ReadLine());
        datos = stream.ReadLine();
        stream.BaseStream.Flush();
    }
   

    public void Close()
    {
        stream.Close();
    }
}