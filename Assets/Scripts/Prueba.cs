using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prueba : MonoBehaviour {

    public GameObject player;
    public ArduinoConnector info;
    public float limitSup;
    public float limitInf;
    public float limitDer;
    public float limitIz;
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
    float rotYant = 0f;
    string rotX = "";
    string rotY = "";
    string accX = "";
    string accY = "";
    string accZ = "";

    // Update is called once per frame
    void Update() {


        int comas = 0;
        int separa = 0;
    

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
                }/*
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

                }*/
            }
        }


        rotationX = float.Parse(rotX);
        rotationY = float.Parse(rotY);
        accelX = float.Parse(accX);

        Ajuste();
        Mover();

        rotX = "";
        rotY = "";
        accX = "";


    }

    private void Mover() {

        player.transform.localEulerAngles = new Vector3(rotationX, 0, rotationY);
        //player.transform.Translate(-(rotationY/10f) * Time.deltaTime, -(rotationX/25f ) * Time.deltaTime, 0);
        

    }



    private void Ajuste()
    {
        ciclo++;

        /*if (accelX >= Calibrador.GetGravedad()*0.9) { rotationY = 0;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(rotationX,0,rotationY), 3 * Time.deltaTime, 0.0F);
            player.transform.rotation = Quaternion.LookRotation(newDir);
        }*/
      //  if (rotationY + rotYant < 2.5 && rotationY - rotYant > -2.5f) { rotYant = rotationY; rotationY = 0; }
        rotXDatos[ciclo] = rotationX;
        rotYDatos[ciclo] = rotationY;
        float datosXcount = 0;
        float datosYcount = 0;

        rotYant = rotationY;
        for (int i = 0; i <= 4; i++) {

            datosXcount = datosXcount + rotXDatos[i];
            datosYcount = datosYcount + rotYDatos[i];


        }

        rotationX = datosXcount / 5f;
        rotationY = datosYcount / 5f;


        if (ciclo== 4) {

            ciclo = 0;
        }

        if (Mathf.Abs(rotationX) > Mathf.Abs(rotationY))
        {

            
            player.transform.position += Vector3.up * (-rotationX) * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(rotationX, 0, rotationY), 1000 * Time.deltaTime, 3F);
            player.transform.rotation = Quaternion.LookRotation(newDir);
        }
        else {
            rotationX = 0;
            player.transform.position += Vector3.right * (-rotationY) * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(rotationX, 0, rotationY), 1000 * Time.deltaTime, 3F);
            player.transform.rotation = Quaternion.LookRotation(newDir);
        }
        if (player.transform.position.y > limitSup) {
            player.transform.position = new Vector3(player.transform.position.x, limitSup, player.transform.position.z);
        }
        else if (player.transform.position.y < limitInf) {

            player.transform.position = new Vector3(player.transform.position.x, limitInf, player.transform.position.z);
        }


        if (player.transform.position.x > limitDer)
        {
            player.transform.position = new Vector3(limitDer, player.transform.position.y, player.transform.position.z);
        }
        else if (player.transform.position.x < limitInf)
        {

            player.transform.position = new Vector3(limitIz, player.transform.position.y, player.transform.position.z);
        }
    }


}
