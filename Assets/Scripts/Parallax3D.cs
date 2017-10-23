using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax3D : MonoBehaviour {

    public int speed;
    public Transform destructor;
    public Transform generador;
    public float offset, inrementoVelocidadConTiempo;

    private static float tiempoInicial;

    void Awake()
    {
        tiempoInicial = Time.time;
    }


	// Update is called once per frame
	void Update () {
        float translation = Time.deltaTime * speed + (Time.time - tiempoInicial) * inrementoVelocidadConTiempo;
        this.transform.Translate(Vector3.back * translation);
        if (transform.position.z <= destructor.position.z) transform.position = generador.position - new Vector3(0, 0, offset);
	}

    public void SetTiempoInicial(float tiempo)
    {
        tiempoInicial = tiempo;
    }

    public static void Reset()
    {
        tiempoInicial = Time.time;
    }
}
