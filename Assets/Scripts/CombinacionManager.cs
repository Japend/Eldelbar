using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class CombinacionManager : MonoBehaviour {

    //Esta clase se le añade a la cámara, por ejemplo. Trata las entradas de Arduino y crea las combinaciones (optimización de acceso a variables...)

    /*
     * CREAR UN ARRAY DE ENTEROS DE X ELEMENTOS (DEPENDIENDO DE LA DIFICULTAD) CON NÚMEROS RANDOM DEL 1 AL 4
     * --> TEMPORIZADOR
     * CUANDO SE TOCA ALGÚN US, COMPARA CON EL PRIMER ELEMENTO Y
     *  SI ES CORRECTO, ++ ÍNDICE, SE COLOREA Y SIGUE ESPERANDO
     *  SI ES INCORRECTO, INDICE A 0 Y TODOS SE APAGAN
     *  */



    public String datos;
    public String port;
    public int frequency = 38400;
    public Char separador = '_';
    public int numFuentes = 4;

    private SerialPort stream;
    public String[] pines;
    Renderer myRenderer;
    public /*static*/ bool[] pinesActivos;

    /*************************/

    public int[] combinacion;
    int nivelInicial = 1; //Indica los elementos a pulsar seguidos (habrá que hacer el temporizador en función a esto también)
    int indiceActual = 0; // Índice en el que está comprobando si se ha pulsado (En array "combinación")
    int nivelActual;


	// Use this for initialization
	void Awake () {
        combinacion = new int[nivelInicial];
        NuevaCombinacion();

        datos = "";
        stream = new SerialPort(port, frequency);
        stream.Open();

        pines = new String[numFuentes];
        pinesActivos = new bool[numFuentes];
        for (int i = 0; i < pinesActivos.Length; i++) pinesActivos[i] = false;
        myRenderer = this.GetComponent<Renderer>();
        nivelActual = nivelInicial;
    }


    void Update()
    {
        datos = stream.ReadLine(); //Coge los datos del buffer
        stream.BaseStream.Flush(); //Vacía el buffer
        pines = ArduinoSplit(datos, separador); //Guarda en cada posición del array la distancia de cada ultrasonido           

        //ComprobarCombinacion();
            
    }

    private String[] ArduinoSplit(String cadenaDatos, char separador)
    {
        String[] _pines = new String[numFuentes];
        int barras = 0;

        foreach (char dato in cadenaDatos) //De esta forma se recorre solo una vez la cadena de Datos
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

    public /*static*/ bool[] GetPinesActivos()
    {
        return pinesActivos;
    }


    void ComprobarCombinacion()
    {
        //Si se mira ese pin y está descativado, paso
        if (pinesActivos[combinacion[indiceActual]] == false) return;

        for (int i=0; i<pinesActivos.Length; i++)
        {
            //Si está activado, compruebo que sea el único (Si hay alguno activado salgo directamente, ya no me vale
            if (i != combinacion[indiceActual] && pinesActivos[i] == true)
            {
                indiceActual = 0;
                return;
            }
        }


        if(indiceActual == nivelActual - 1) //Ha llegado hasta el final del array --> Ha acertado todos en el orden correcto
        {
            //Debug.Log("CORRECTO!");
        }


    }

    void NuevaCombinacion()
    {
        for(int i = 0; i<combinacion.Length; i++)
        {
            combinacion[i] = UnityEngine.Random.Range(0, 3); //Número random entre 0 y 3 (indica los pines que se deben pulsar)
        }
    }
}

/*     
            
            
            
            //Si se está mirando otro pin y está a true...
            if (i != combinacion[indiceActual] && pinesActivos[i])
            {
                indiceActual = 0;
                return; //Sale de la función y vuelta a empezar
            }

            //Se está mirando el pin que debe estar activo y en efecto, está activo...
            if (i == combinacion[indiceActual] && pinesActivos[i])
            {
                indiceActual++; //Se pasa al siguiente a comprobar
            }
*/