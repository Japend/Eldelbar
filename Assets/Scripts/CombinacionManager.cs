using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using UnityEngine.UI;
using System.Threading;


public class CombinacionManager : MonoBehaviour {

    //Esta clase se le añade a la cámara, por ejemplo. Trata las entradas de Arduino y crea las combinaciones (optimización de acceso a variables...)

    /*
     * CREAR UN ARRAY DE ENTEROS DE X ELEMENTOS (DEPENDIENDO DE LA DIFICULTAD) CON NÚMEROS RANDOM DEL 1 AL 4
     * --> TEMPORIZADOR
     * CUANDO SE TOCA ALGÚN US, COMPARA CON EL PRIMER ELEMENTO Y
     *  SI ES CORRECTO, ++ ÍNDICE, SE COLOREA Y SIGUE ESPERANDO
     *  SI ES INCORRECTO, INDICE A 0 Y TODOS SE APAGAN
     *  */

    public Canvas canvas;
    public static Transform target; //La pared de la puerta
    private Camera camera;
    public static bool probandoCombinacion;
    public string datos;
    public String port;
    public int frequency = 38400;
    public Char separador = '_';
    public int numFuentes = 4;

    private SerialPort stream;
    public String[] pines;
    Renderer myRenderer;
    public static bool[] pinesActivos;

    /*************************/
    public int nivelInicial = 2; //Indica los elementos a pulsar seguidos (habrá que hacer el temporizador en función a esto también)
    public int indiceActual = 0; // Índice en el que está comprobando si se ha pulsado (En array "combinación")
    public int nivelActual;

    public int[] combinacion;
    public bool[] acertados;

    public Text textoCombinacion;
    public Text textoCombinacionTuya;

    private Thread hilo;
    bool stopThread = false;

    public bool permitePulsar;
    public float temp;
    public float tiempoEntrePulsaciones = 0.15f;


	// Use this for initialization
	void Awake () {

        camera = GetComponent<Camera>();
        nivelActual = nivelInicial;        
        NuevaCombinacion();

        GlobalData.DatosUltrasonidos = "";
        permitePulsar = true;
        temp = 0;

        pines = new String[numFuentes];
        pinesActivos = new bool[numFuentes];
        acertados = new bool[nivelInicial]; //Tendrá que crearse un nuevo array a cada nivel

        for (int i = 0; i < pinesActivos.Length; i++) pinesActivos[i] = false;
        myRenderer = this.GetComponent<Renderer>();
        canvas = GameObject.Find("CanvasCombinacion").GetComponent<Canvas>();
        canvas.enabled = false;

        hilo = new Thread(funcionHiloUltrasonidos);
        hilo.Start();        
    }


    void ComprobarParedVisible()
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(target.transform.position);
        //bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        bool onScreen = target.position.z <= 155 && target.position.z >= 100;

        if (onScreen)
        {
            //target.GetComponent<Renderer>().material.color = Color.blue;
            if(! canvas.isActiveAndEnabled) textoCombinacionTuya.text = ""; //Seguridad
            canvas.enabled = true;
            probandoCombinacion = true;

            if(Time.timeScale > 0.3f)
                Time.timeScale = 0.2f;        
            //RALENTIZAR UN DETERMINADO TIEMPO HASTA QUE SE HAGA LA COMBINACIÓN
        }
        else canvas.enabled = false;

    }


    void Update()
    {
        datos = GlobalData.DatosUltrasonidos;
        pines = ArduinoSplit(GlobalData.DatosUltrasonidos, separador); //Guarda en cada posición del array la distancia de cada ultrasonido           

        if (temp >= tiempoEntrePulsaciones)
        {
            ComprobarCombinacion();
            temp = 0;
        }
        else temp += Time.deltaTime;

        ComprobarParedVisible();
    }

    private String[] ArduinoSplit(String cadenaDatos, char separador)
    {
        String[] _pines = new String[numFuentes];
        int barras = 0;

        foreach (char dato in cadenaDatos) //De esta forma se recorre solo una vez la cadena de GlobalData.DatosUltrasonidos
        {
            if (dato.Equals(separador)) //Ya ha acabado un número
            {
                pinesActivos[barras] = (_pines[barras].Length > 1) ? false : true; //   OJO: Si es mayor que uno, no está activo
                barras++;
            }
            else
            {
                _pines[barras] += dato;
            }
        }

        pinesActivos[numFuentes - 1] = (_pines[barras].Length > 1) ? false : true; //Comprueba el último elemento
        return _pines;
    }

    public void funcionHiloUltrasonidos()
    {
        stream = new SerialPort(port, frequency);
        stream.Open();

        while(hilo.IsAlive && !stopThread)
        {
            GlobalData.DatosUltrasonidos = stream.ReadLine(); //Coge los GlobalData.DatosUltrasonidos del buffer
            stream.DiscardInBuffer();
            stream.BaseStream.Flush();

        }
        stream.Close();
    }

    void ComprobarCombinacion()
    {
        //Si se mira ese pin y está descativado, paso
        //if (pinesActivos[combinacion[indiceActual]] == false) return; 
        //ESTO ESTÁ MAL, PORQUE EN EL MOMENTO EN QUE NO ESTÉ ACTIVO NO LO COMPRUEBA HASTA QUE ESTÉ ACTIVO. DE ESTA FORMA NUNCA SE REINICIA

        for (int i=0; i<pinesActivos.Length; i++)
        {
            //Si está activado, compruebo que sea el único (Si hay alguno activado salgo directamente, ya no me vale
            if (i != combinacion[indiceActual] && pinesActivos[i] == true)
            {
                textoCombinacionTuya.text = "";
                indiceActual = 0; //En el momento en que fallas, se reinicia
                for (int a = 0; a < acertados.Length; a++) acertados[a] = false;
                return;
            }
        }

        if(pinesActivos[combinacion[indiceActual]] == true) //ACIERTA UNO DE LOS NÚMEROS
        {
            acertados[indiceActual] = true;
            textoCombinacionTuya.text += combinacion[indiceActual] + "  ";
            indiceActual++; //Se actualiza el índice
        }
        
        //ES CORRECTA **********************************
        if(indiceActual == nivelActual) //Ha llegado hasta el final del array --> Ha acertado todos en el orden correcto
        {
            //Debug.Log("CORRECTO!");

            //target.GetComponent<Renderer>().material.color = Color.green;
            target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -25f);
            Time.timeScale = 1f;
            probandoCombinacion = false;
            canvas.enabled = false; //Se desactiva el canvas  
            nivelActual++;
            textoCombinacion.text = "";
            NuevaCombinacion();                      
        }
    }

    void NuevaCombinacion()
    {
        //REINICIAMOS VARIABLES
        combinacion = new int[nivelActual]; //Tendrá que crearse un nuevo array a cada nivel
        acertados = new bool[nivelActual];
        indiceActual = 0; //Reinicia el índice
        textoCombinacionTuya.text = "";


        for (int i = 0; i<combinacion.Length; i++)
        {
            combinacion[i] = UnityEngine.Random.Range(0, 3); //Número random entre 0 y 3 (indica los pines que se deben pulsar)                        
        }

        for (int k = 0; k < combinacion.Length; k++)
        {
            textoCombinacion.text += combinacion[k];
                if (k != combinacion.Length - 1) textoCombinacion.text += " - ";
        }

    }



    public static bool[] GetPinesActivos()
    {
        return pinesActivos;
    }

    void OnApplicationQuit()
    {
        Debug.Log("STOPPED");
        stopThread = true;
    }

    public void Reset()
    {
        nivelActual = 2;
        canvas.enabled = false;
        
    }


}