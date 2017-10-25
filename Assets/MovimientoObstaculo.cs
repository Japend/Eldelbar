using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoObstaculo : MonoBehaviour {

    private const float VELOCIDAD_ROTACION = 1f;

    bool rotX, rotY, rotZ;
    Vector3 eulerRotOriginal;
    Vector3 rotActual;


    void Awake()
    {
        eulerRotOriginal = transform.rotation.eulerAngles;
    }

	// Use this for initialization
	void Start () {
        rotActual = eulerRotOriginal;
        //cada vez que se active rotara en ejes distintos
        if (Random.value > 0.5f)
            rotX = true;
        else
            rotX = false;

        if (Random.value > 0.5f)
            rotY = true;
        else
            rotY = false;


        if (Random.value > 0.5f)
            rotZ = true;
        else
            rotZ = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (rotX)
            rotActual.x += VELOCIDAD_ROTACION;
        if (rotY)
            rotActual.y += VELOCIDAD_ROTACION;
        if (rotZ)
            rotActual.z += VELOCIDAD_ROTACION;

        transform.localEulerAngles = rotActual;
	}
}
