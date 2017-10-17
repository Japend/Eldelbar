using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax3D : MonoBehaviour {

    public int speed;
    public Transform destructor;
    public Transform generador;
    public float offset;
       

	// Update is called once per frame
	void Update () {
        float translation = Time.deltaTime * speed;
        this.transform.Translate(Vector3.back * translation);
        if (transform.position.z <= destructor.position.z) transform.position = generador.position - new Vector3(0, 0, offset);
	}
}
