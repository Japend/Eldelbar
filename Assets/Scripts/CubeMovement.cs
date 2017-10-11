using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour {

    public GameObject player;
    public ArduinoConnector info;
    private float rotationX;
    private float rotationY;
    private float accelX;
   //private float accelY;
    private float accelZ;
    public float speed = 1f;
    private float accelation = 0.5f;
    private Vector3 accel;
    private float[] rotXDatos = new float[5] { 0,0,0,0,0};
    private float[] rotYDatos = new float[5] { 0, 0, 0, 0, 0 };
    private float[] accXDatos = new float[5] { 0, 0, 0, 0, 0 };
    //private float[] accYDatos = new float[5] { 0, 0, 0, 0, 0 };
    private float[] accZDatos = new float[5] { 0, 0, 0, 0, 0 };
    int ciclo = 0;
    float rotXant = 0f;
    float rotYant = 0f;
    float limitX = 0f;








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
        Ajuste();
        player.transform.localEulerAngles = new Vector3(rotationX, 0,rotationY);
        Mover();



    }


    private void Ajuste()
    {
        ciclo++;

        if (rotationX < -15f && rotationX > 15f) { rotationX = 0f; }
        if (rotationY < -15f && rotationY > 15f) { rotationY = 0f; }
        rotXDatos[ciclo] = rotationX;
        rotYDatos[ciclo] = rotationY;
        float datosXcount = 0;
        float datosYcount = 0;

        for (int i = 0; i <= 4; i++) {

            datosXcount = datosXcount + rotXDatos[i];
            datosYcount = datosYcount + rotYDatos[i];


        }

        rotationX = datosXcount / 5f;
        rotationY = datosYcount / 5f;


        if (ciclo== 4) {

            ciclo = 0;
        }
    }

    private void Mover() {

        if (rotXant > player.transform.localEulerAngles.x) {

            player.transform.position += Vector3.left + new Vector3(Mathf.Sin(player.transform.localEulerAngles.x)/10,0,0);
            

        }
        else if (rotXant < player.transform.localEulerAngles.x) {
            player.transform.position += Vector3.right + new Vector3(Mathf.Sin(player.transform.localEulerAngles.x)/10, 0, 0);
            
        }


        rotXant = player.transform.localEulerAngles.x;
    }


}
