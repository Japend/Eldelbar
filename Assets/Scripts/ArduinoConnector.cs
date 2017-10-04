/* ArduinoConnector by Alan Zucconi
 * http://www.alanzucconi.com/?p=2979
 */
using UnityEngine;
using System;
using System.IO.Ports;

public class ArduinoConnector : MonoBehaviour
{

    private SerialPort stream;
    public String datos;
    public String port;
    public int frequency;

    public void Start()
    {
        // Opens the serial port
        datos = "";
        stream = new SerialPort(port, frequency);
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