using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatosGlobales{

	public const int MENU_PRINCIPAL = 0;
	public const int JUGANDO = 1;
	public const int PAUSA = 2;
	public const int CALIBRANDO = 3;
	public const int FIN_DEL_JUEGO = 4;

	public static int EstadoJuego = 0;


	private static int puntuacion = 0;

	public static int IncrementarPuntuacion(int cantidad)
	{
		puntuacion += cantidad;
		return puntuacion;
	}

	public static int GetPuntuacion()
	{
		return puntuacion;
	}
}
