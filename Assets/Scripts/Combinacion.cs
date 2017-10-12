using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combinacion : MonoBehaviour {

    Text text;
    public int pin;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        //text.color = (CombinacionManager.GetPinesActivos()[pin]) ? Color.red : Color.black;
	}
}
