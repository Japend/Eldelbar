using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenuPrincipal : MonoBehaviour {

	private GameObject continuar;
	private GameObject empezar;
	private GameObject salir;
	private GameObject calibrar;
	private GameObject calibrando;
    private GameObject puntuacion;
    private GameObject logo;
    private GameObject puntuacionFinJuego;

    private CombinacionManager cm;
    private Prueba bandeja;
    private GeneradorObstaculos generadorObstaculos;

    public string[] datos;
    private bool[] bootonesPulsados;

	int estadoAnterior;

	void Awake () {
		continuar = GameObject.Find ("Continuar");
        puntuacion = GameObject.Find("Puntuacion");
		empezar = GameObject.Find ("Empezar");
        logo = GameObject.Find("LogoMenu");
		salir = GameObject.Find ("Salir");
		calibrando = GameObject.Find ("Calibrando");
		estadoAnterior = -1;  //para que se ejecute el switch la primera vez
        bootonesPulsados = new bool[4];
        generadorObstaculos = GameObject.Find("Generatron").GetComponent<GeneradorObstaculos>();
        puntuacionFinJuego = GameObject.Find("PuntuacionFinJuego");
        cm = GameObject.Find("Main Camera").GetComponent<CombinacionManager>();
        bandeja = GameObject.Find("Bandeja").GetComponent<Prueba>();
	}
		

	void Update () {

        puntuacion.GetComponentInChildren<UnityEngine.UI.Text>().text = System.Convert.ToString(GlobalData.GetPuntuacion());

        datos = GlobalData.DatosUltrasonidos.Split('_');

        if(System.Convert.ToInt32(datos[0]) < 15)
            bootonesPulsados[0] = true;
        else
            bootonesPulsados[0] = false;

        if (System.Convert.ToInt32(datos[1]) < 15)
            bootonesPulsados[1] = true;
        else
            bootonesPulsados[1] = false;

        if (System.Convert.ToInt32(datos[2]) < 15)
            bootonesPulsados[2] = true;
        else
            bootonesPulsados[2] = false;

        if (System.Convert.ToInt32(datos[3]) < 15)
            bootonesPulsados[3] = true;
        else
            bootonesPulsados[3] = false;


        //acciones menu principal
        if (GlobalData.EstadoJuego == GlobalData.MENU_PRINCIPAL)
        {
            if (bootonesPulsados[0])
                GlobalData.EstadoJuego = GlobalData.JUGANDO;
            else if (bootonesPulsados[3])
                Application.Quit();
        }

        //acciones juego en curso
        if (GlobalData.EstadoJuego == GlobalData.JUGANDO)
        {
            if (bootonesPulsados[0] && bootonesPulsados[3])
                GlobalData.EstadoJuego = GlobalData.PAUSA;
        }

        if (GlobalData.EstadoJuego == GlobalData.PAUSA)
        {
            if (bootonesPulsados[1])
                GlobalData.EstadoJuego = GlobalData.JUGANDO;
            else if (bootonesPulsados[2])
            {
                reset();
                GlobalData.EstadoJuego = GlobalData.MENU_PRINCIPAL;
            }
        }

        if (GlobalData.EstadoJuego == GlobalData.FIN_DEL_JUEGO)
        {
            if (bootonesPulsados[1])
                GlobalData.EstadoJuego = GlobalData.MENU_PRINCIPAL;
            if (bootonesPulsados[3])
                Application.Quit();
        }

		if (estadoAnterior != GlobalData.EstadoJuego) {
            estadoAnterior = GlobalData.EstadoJuego;

            switch (GlobalData.EstadoJuego)
            {

                case GlobalData.MENU_PRINCIPAL:
                    puntuacionFinJuego.SetActive(false);
                    puntuacion.SetActive(false);
				    empezar.SetActive (true);
                    salir.GetComponentInChildren<UnityEngine.UI.Text>().text = "SALIR - 4";
				    salir.SetActive (true);
                    logo.SetActive(true);
				    calibrando.SetActive (true);
                    continuar.SetActive(false);
                    Time.timeScale = 0f;
				    break;

                case GlobalData.JUGANDO:
                    calibrando.SetActive(false);
                    empezar.SetActive(false);
                    salir.SetActive(false);
                    logo.SetActive(false);
                    continuar.SetActive(false);
                    puntuacion.SetActive(true);
				    Time.timeScale = 1f;
				    break;

                case GlobalData.PAUSA:
				    this.gameObject.GetComponent<Canvas> ().enabled = true;
				    empezar.SetActive (false);
                    salir.GetComponentInChildren<UnityEngine.UI.Text>().text = "SALIR - 3";
				    salir.SetActive (true);
				    calibrando.SetActive (true);
                    logo.SetActive(true);
				    continuar.SetActive (true);
				    Time.timeScale = 0f;
				    break;

                case GlobalData.FIN_DEL_JUEGO:
                    this.gameObject.GetComponent<Canvas>().enabled = true;
                    empezar.SetActive (false);
                    salir.GetComponentInChildren<UnityEngine.UI.Text>().text = "SALIR - 4";
                    puntuacionFinJuego.GetComponent<UnityEngine.UI.Text>().text = "PUNUACIÓN FINAL: " + System.Convert.ToString(GlobalData.GetPuntuacion());
                    puntuacionFinJuego.SetActive(true);
                    continuar.SetActive (true);
			        Time.timeScale = 0f;
			        salir.SetActive (true);
                    reset();
                    break;
			}
		}

	}

    private void reset()
    {
        generadorObstaculos.Reset();
        GlobalData.ResetPuntuacion();
        Parallax3D.Reset();
        cm.Reset();
        bandeja.Resetear();
    }
}
