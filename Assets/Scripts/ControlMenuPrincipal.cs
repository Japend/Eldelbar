using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenuPrincipal : MonoBehaviour {

	private GameObject continuar;
	private GameObject empezar;
	private GameObject salir;
	private GameObject calibrar;
	private GameObject calibrando;

	int estadoAnterior;

	void Awake () {
		continuar = GameObject.Find ("Continuar");
		empezar = GameObject.Find ("Empezar");
		calibrar = GameObject.Find ("Calibrar");
		salir = GameObject.Find ("Salir");
		calibrando = GameObject.Find ("Calibrando");
		estadoAnterior = -1;  //para que se ejecute el switch la primera vez
	}
		

	void Update () {

		if (estadoAnterior != DatosGlobales.EstadoJuego) {
			estadoAnterior = DatosGlobales.EstadoJuego;

			switch (DatosGlobales.EstadoJuego) {

			case DatosGlobales.MENU_PRINCIPAL:
				empezar.SetActive (true);
				salir.SetActive (true);
				calibrar.SetActive (true);
				calibrando.SetActive (false);
				continuar.SetActive (false);
				Time.timeScale = 0;
				break;

			case DatosGlobales.JUGANDO:
				this.gameObject.GetComponent<Canvas> ().enabled = false;
				Time.timeScale = 0;
				break;

			case DatosGlobales.PAUSA:
				this.gameObject.GetComponent<Canvas> ().enabled = true;
				empezar.SetActive (false);
				salir.SetActive (true);
				calibrar.SetActive (true);
				calibrando.SetActive (false);
				continuar.SetActive (true);
				Time.timeScale = 0;
				break;

			case DatosGlobales.CALIBRANDO:
				empezar.SetActive (false);
				salir.SetActive (false);
				calibrar.SetActive (false);
				calibrando.SetActive (true);
				continuar.SetActive (false);
				break;
			}
		}

		//codigo para leer ultrasonidos y modificar stado del juego
	}
}
