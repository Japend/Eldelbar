/* ArduinoConnector by Alan Zucconi
 * http://www.alanzucconi.com/?p=2979
 */
using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;

public class ArduinoConnector : MonoBehaviour
{

    private SerialPort stream;
    public String datos;
    public String port;
    public int frequency;

    private Thread hilo;
    private bool killThrad;

    public void Start()
    {
        hilo = new Thread(funcionHiloAcelerometro);
        hilo.Start();
        // Opens the serial port
        datos = "0.00,0.00|0";
        
    }

    public void Update()
    {

        //Debug.Log(stream.ReadLine());
        
    }


    public void OnApplicationQuit()
    {
        killThrad = true;
    }

    private void funcionHiloAcelerometro()
    {
        stream = new SerialPort(port, frequency);
        stream.Open();

        while(!killThrad)
        {
            datos = stream.ReadLine();
            stream.DiscardInBuffer();
            stream.BaseStream.Flush();
        }
        stream.Close();
    }
}