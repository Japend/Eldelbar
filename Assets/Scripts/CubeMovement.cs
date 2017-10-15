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
    private float[] rotXDatos = new float[5] { 0, 0, 0, 0, 0 };
    private float[] rotYDatos = new float[5] { 0, 0, 0, 0, 0 };
    private float[] accXDatos = new float[5] { 0, 0, 0, 0, 0 };
    //private float[] accYDatos = new float[5] { 0, 0, 0, 0, 0 };
    private float[] accZDatos = new float[5] { 0, 0, 0, 0, 0 };
    int ciclo = 0;
    float rotXant = 0f;
    float rotYant = 0f;
    float limitX = 0f;
    float movimientoX = 0f;

    // Update is called once per frame
    void Update() {


        int comas = 0;
        int separa = 0;
        string rotX = "";
        string rotY = "";
        string accX = "";
        string accY = "";
        string accZ = "";

        foreach (char dato in info.datos) {
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

        if (DatosGlobales.EstadoJuego == DatosGlobales.CALIBRANDO)
        {
            if (Calibrador.Calibrar(rotationX, rotationY, accelX))
                DatosGlobales.EstadoJuego = DatosGlobales.PAUSA;
        }

        else if(DatosGlobales.EstadoJuego == DatosGlobales.JUGANDO)
        {
            Ajuste();
            Mover();
        }


    }

    private void Mover() {

        player.transform.localEulerAngles = new Vector3(rotationX, 0, rotationY);
        player.transform.Translate(-(rotationY/50), -(rotationX / 100), 0);
        

    }



    private void Ajuste()
    {
        ciclo++;
        if (rotationY < 10 && rotationY > -20) { rotationY = 0; }
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


}
