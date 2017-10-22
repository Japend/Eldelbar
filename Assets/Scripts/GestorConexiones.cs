using UnityEngine;
using System.Threading;
using System.IO.Ports;
using System.Collections.Generic;

public class GestorConexiones : MonoBehaviour{

    public int frecuencia;

    Thread threadGiroscopio, threadUltrasonidos;
    bool killThread = false;
    string puertoGyro, puertoUltra;
    SerialPort streamGyro, streamUltra;

    public void Awake()
    {
        SerialPort streamAux;
        string aux;

        foreach (string s in SerialPort.GetPortNames())
        {
            streamAux = new SerialPort(s, frecuencia);
            streamAux.Open();
            aux = streamAux.ReadLine();

            if(aux == GlobalData.ID_ACELEROMETRO)
                puertoGyro = s;
            else if( aux == GlobalData.ID_ULTRASONIDOS)
                puertoUltra = s;
        }

        threadGiroscopio = new Thread(funcionHiloAcelerometro);

    }

    public void OnApplicationQuit()
    {
        killThread = true;
    }

    private void funcionHiloAcelerometro()
    {
        try
        {
            streamGyro = new SerialPort(puertoGyro, frecuencia);
            streamGyro.Open();

            while (!killThread)
            {
                GlobalData.DatosGiroscopio = streamGyro.ReadLine();
                streamGyro.DiscardInBuffer();
                streamGyro.BaseStream.Flush();
            }
            streamGyro.Close();
        }

        catch (ThreadAbortException e){
            streamGyro.Close();
        }
    }


    private void funcionHiloUltrasonidos()
    {
        streamUltra = new SerialPort(puertoUltra, frecuencia);
        streamUltra.Open();

        while (!killThread)
        {
            GlobalData.DatosUltrasonidos = streamUltra.ReadLine();
            streamUltra.DiscardInBuffer();
            streamUltra.BaseStream.Flush();
        }
        streamUltra.Close();
    }


    public void Calibrar()
    {
        //reinicia el thread del giroscopio para que se reclaibr ele valor de la gravedad
        threadGiroscopio.Abort();
        threadGiroscopio.Start();
    }
}
