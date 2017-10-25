using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class efectoGiro : MonoBehaviour {

public float turnSpeed;
public char eje;
Vector3 vectorGiro;
	// Use this for initialization
	void Start () {
		switch(eje){
			case 'x':
			vectorGiro = Vector3.right;
			break;
			case 'y':
			vectorGiro = Vector3.up;
			break;
			case 'z':
			vectorGiro = Vector3.forward;
			break;
			case 'a':
			vectorGiro = new Vector3(1, 1, 1);
			break;


		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(vectorGiro, turnSpeed * Time.deltaTime);
	}
}
