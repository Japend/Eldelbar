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
        //accelY = float.Parse(accY);
        accelZ = float.Parse(accZ);
        Debug.Log(accelZ);
        DeleteRuido();
        Ajuste(rotationX,rotationY,accelX,  0,accelZ); 
        //accel = new Vector3(accelX, 0, 0);
        player.transform.localEulerAngles = new Vector3(rotationX, 0,rotationY);
        //player.transform.Translate(accel);



    }


    private void Ajuste(float rotationX, float rotationY, float accelX, float accelY, float accelZ) {

        rotXDatos[ciclo] = rotationX;
        rotYDatos[ciclo] = rotationY;
        accXDatos[ciclo] = accelX;
        accZDatos[ciclo] = accelZ;

        ciclo++;
        if (ciclo > 4) {
            ciclo = 0;
        }
        Mezclar();
    }

    private void Mezclar() {

        /*float counterRotX = 0f;
        float counterRotY = 0f;
        float counterAccelX = 0f;
        float counterAccelZ = 0f;



        for (int i = 0; i < 5; i++) {

            counterRotX = counterRotX + rotXDatos[i];
            counterRotY = counterRotY + rotYDatos[i];
            counterAccelX = counterAccelX + accXDatos[i];
            counterAccelZ = counterAccelZ + accZDatos[i];

        }

        rotationX = counterRotX / 5f;
        rotationY = counterRotY / 5f;
        accelX = accelX / 5f;
        accelZ = accelZ / 5f;*/
        //Ajuste
        int ant;
        if (ciclo == 0)
        {
            ant = 4;
        }
        else {

            ant = ciclo - 1;
        }
        //Aplicar Velocidad segun la dirección
        if (accXDatos[ciclo] > accXDatos[ant] + 0.6f)
        {

            player.transform.Translate(2,0,0);
            

        }
        else if (accXDatos[ciclo] < accXDatos[ant]-0.6f)
        {

            player.transform.Translate(-2, 0, 0);


        }

        if (accZDatos[ciclo] > accZDatos[ant] + 0.6f)
        {

            player.transform.Translate(0, -2, 0);

        }
        else if (accZDatos[ciclo] < accZDatos[ant] - 0.6f)
        {

            player.transform.Translate(0, 2, 0);


        }


    }

    private void DeleteRuido() {

        if (accelX > -0.15f && accelX < 0.15f) {

            accelX = 0f;
        }


        if (accelZ > -0.15f && accelZ < 0.15f)
        {

            accelZ = 0f;
        }

    }
}
