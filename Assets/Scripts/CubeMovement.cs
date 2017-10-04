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
        accelZ = float.Parse(accZ);

        //player.transform.localEulerAngles = new Vector3(rotationX, 0,0);
        //player.transform.localEulerAngles = new Vector3(0, 0, rotationY);
        //player.transform.position = new Vector3(accelX,accelY, accelZ);


    }

}
