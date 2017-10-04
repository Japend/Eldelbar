using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubesTest : MonoBehaviour {

    public String[] pines;
    public int pinUsed;
    Renderer myRenderer;

    public ArduinoConnector info;

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

        LightCube();
        //MoveCube();
	}

    void MoveCube()
    {
        if (pines[pinUsed - 1] != null)
        {
            Vector3 newPosition = new Vector3(transform.position.x,(45 - int.Parse(pines[pinUsed - 1])), transform.position.z);
            transform.position = newPosition;
        }
    }

    void LightCube()
    {
        if (pines[pinUsed - 1] != null && pines[pinUsed - 1] != "45")
        {
            myRenderer.material = myRenderer.materials[1];
            //Debug.Log("Iluminado");
        }
        else 
            myRenderer.material = myRenderer.materials[0];
    }
}
