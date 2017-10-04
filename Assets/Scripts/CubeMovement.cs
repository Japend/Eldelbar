using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour {

    public GameObject player;
    public ArduinoConnector info;
    private float rotationX;
    private float rotationY;
    private float accelX;
    private float accelY;
    private float accelZ;
    public float speed = 20f;
    private Vector3 accel;
    private float[] rotXDatos = new float[5] { 0,0,0,0,0};
    private float[] rotYDatos = new float[5] { 0, 0, 0, 0, 0 };
    private float[] accXDatos = new float[5] { 0, 0, 0, 0, 0 };
    private float[] accYDatos = new float[5] { 0, 0, 0, 0, 0 };
    private float[] accZDatos = new float[5] { 0, 0, 0, 0, 0 };
    int ciclo = 0;








    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        int comas = 0;
        int separa = 0;
        string rotX = "";
        string rotY = "";
        string accX = "";
        string accY = "";
        string accZ = "";

        foreach ( char dato in info.datos) {
            if (separa == 0) //Rotaciones
            {
                if (comas == 0) //Rotacion en X
                {   
                    if (dato.Equals(','))
                    {
                        comas++;


                    }
                    else
                    {
                        rotX = rotX + dato;

                    }
                }
                else //Rotacion en Y
                {
                    if (dato.Equals('|'))
                    {

                        comas = 0;
                        separa++;
                        
                    }
                    else {

                        rotY = rotY + dato;
                    }

                }
            }
            else { //Aceleraciones

                if (comas == 0)
                { //aceleracion X

                    if (dato.Equals(','))
                    {

                        comas++;
                    }
                    else
                    {

                        accX = accX + dato;
                    }
                }
                else if (comas == 1)
                {

                    if (dato.Equals(','))
                    {

                        comas++;
                    }
                    else
                    {

                        accY = accY + dato;
                    }
                }
                else {

                    accZ = accZ + dato;

                }
            }
        }

        

        rotationX = float.Parse(rotX);
        rotationY = float.Parse(rotY);
        accelX = float.Parse(accX);
        accelY = float.Parse(accY);
        accelZ = float.Parse(accZ)/1.5f;

        //accel = new Vector3(accelX, accelY, accelZ);
        //accel = new Vector3(0, -accelZ, 0);
        //accel = new Vector3(-accelX, 0, 0);
        accel = new Vector3(accelX, -accelZ, accelY);

        player.transform.localEulerAngles = new Vector3(rotationX, 0,rotationY);
        //player.transform.localEulerAngles = new Vector3(0, 0, rotationY);
        //player.transform.position = new Vector3(accelX,accelY, accelZ);
        //player.transform.position = new Vector3(accelX, accelY, accelZ) + player.transform.localPosition;
        player.transform.Translate(speed * accel * Time.deltaTime );


    }


    private void Ajuste(float rotationX, float rotationY, float accelX, float accelY, float accelZ) {

        rotXDatos[ciclo] = rotationX;
        rotYDatos[ciclo] = rotationY;
        accXDatos[ciclo] = accelX;
        accYDatos[ciclo] = accelY;
        accZDatos[ciclo] = accelZ;

        ciclo++;
        if (ciclo > 4) {
            ciclo = 0;
        }


    }
}
