using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubesTest : MonoBehaviour {

    public String[] pines;
    Renderer myRenderer;

    public ArduinoConnector info;
    public /*private static*/ bool[] pinesActivos;

    // Use this for initialization
    void Start () {
        pines = new String[4];
        myRenderer = this.GetComponent<Renderer>();

    }
	
	// Update is called once per frame
	void Update () {
        pines = new String[4];
        int barras = 0;
        //pines = info.datos.Split('_');
        foreach(char dato in info.datos)
        {
            if (dato.Equals('_')) barras++;
            else pines[barras] += dato;
        }

        for(int i=0; i<pines.Length; i++)
        {
            String valor = pines[i];
            pinesActivos[i] = (valor != null && System.Convert.ToInt32(valor) < 10) ? true : false;
        }
               
	}

    public /*static*/ bool[] getUltrasoundsArray()
    {
        //Debug.Log(pinesActivos.ToString());
        return pinesActivos;
    }

}
