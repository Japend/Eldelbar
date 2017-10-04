using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubesTest : MonoBehaviour {

    public String[] pines;
    public int pinUsed;

    public ArduinoConnector info;

	// Use this for initialization
	void Start () {
        //pines = new String[4];
	}
	
	// Update is called once per frame
	void Update () {
        pines = info.datos.Split('_');
        transform.position = new Vector3 (transform.position.x, 45 - int.Parse(pines[pinUsed - 1]), transform.position.z);
	}
}
