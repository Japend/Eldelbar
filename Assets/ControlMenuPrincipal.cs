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

		if (estadoAnterior != GlobalData.EstadoJuego) {
			estadoAnterior = GlobalData.EstadoJuego;

			switch (GlobalData.EstadoJuego) {

			case GlobalData.MENU_PRINCIPAL:
				empezar.SetActive (true);
				salir.SetActive (true);
				break;

			}
		}
	}
}
