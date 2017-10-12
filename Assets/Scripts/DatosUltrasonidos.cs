using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class DatosUltrasonidos : MonoBehaviour {

    
    public String datos;
    public String port;
    public int frequency;
    public Char separador = '_';
    public int numFuentes = 4;

    private SerialPort stream;
    public String[] pines;
    Renderer myRenderer;    

    private static bool[] pinesActivos;

    void Awake () {
        datos = "";
        stream = new SerialPort(port, frequency);
        stream.Open();

        pines = new String[numFuentes];
        pinesActivos = new bool[numFuentes];
        myRenderer = this.GetComponent<Renderer>();
    }
	
	void Update () {
        datos = stream.ReadLine(); //Coge los datos del buffer
        stream.BaseStream.Flush(); //Vacía el buffer
        pines = ArduinoSplit(datos, separador); //Guarda en cada posición del array la distancia de cada ultrasonido               
	}

    private String[] ArduinoSplit(String cadenaDatos, char separador)
    {
        String[] _pines = new String[numFuentes];
        int barras = 0;

        foreach (char dato in cadenaDatos) //De esta forma se recorre solo una vez la cadena de Datos
        {
            if (dato.Equals(separador)) //Ya ha acabado un número
            {
                pinesActivos[barras] = (_pines[barras].Length > 1) ? true : false;
                barras++;
            }
            else
            {
                _pines[barras] += dato;
            }
        }

        pinesActivos[numFuentes -1] = (_pines[barras].Length > 1) ? true : false; //Comprueba el último elemento
        return _pines;
    }

    public static bool[] GetPinesActivos()
    {
        return pinesActivos;
    }

}
