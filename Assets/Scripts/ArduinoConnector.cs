/* ArduinoConnector by Alan Zucconi
 * http://www.alanzucconi.com/?p=2979
 */
using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;

public class ArduinoConnector : MonoBehaviour {

    public String port;
    public int frequency;
    private SerialPort stream;

    public void Start () {
        // Opens the serial port
        stream = new SerialPort(port, frequency);
        stream.Open();
    }
    
    public void Update()
    {
        Debug.Log(stream.ReadLine());
        stream.BaseStream.Flush();
    }
   

    public void Close()
    {
        stream.Close();
    }
}