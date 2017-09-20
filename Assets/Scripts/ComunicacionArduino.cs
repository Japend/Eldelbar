using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class ComunicacionArduino : MonoBehaviour {

    SerialPort stream;
    public float timeOut = 0.1f;
    
    public void Awake() {
        stream = new SerialPort("COM3", 9600);
        stream.ReadTimeout = 50;
        stream.Open();
    }

    public void Update()
    {
        WriteToArduino("PING");
           StartCoroutine( AsynchronousReadFromArduino
                ((string s) => Debug.Log(s),     // Callback
                () => Debug.LogError("Error!"), // Error callback
                timeOut                             // Timeout (seconds)
            )
        );
    }


    public void WriteToArduino(string message)
    {
        stream.WriteLine(message);
        stream.BaseStream.Flush();
    }


    
public string ReadFromArduino(int timeout = 0)
    {
        stream.ReadTimeout = timeout;
        try
        {
            return stream.ReadLine();
        }
        catch (System.TimeoutException e)
        {
            return null;
        }
    }
    

    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        do
        {
            try
            {
                dataString = stream.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (dataString != null)
            {
                callback(dataString);
                yield return null;
            }
            else
                yield return new WaitForSeconds(0.05f);

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;

        } while (diff.Milliseconds < timeout);

        if (fail != null)
            fail();
        yield return null;
    }


 


}


 
