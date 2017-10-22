using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData {

    public const string ID_ACELEROMETRO = "0";
    public const string ID_ULTRASONIDOS = "1";

	public const int MENU_PRINCIPAL = 0;
	public const int JUGANDO = 1;
	public const int PAUSA = 2;
	public const int FIN_DEL_JUEGO = 4;

	public static int EstadoJuego = 0;


	private static int puntuacion = 0;

	public static void IncrementarPuntuacion()
	{
        puntuacion++;
	}

	public static int GetPuntuacion()
	{
		return puntuacion;
	}

    public static void ResetPuntuacion()
    {
        puntuacion = 0;
    }
    public static string DatosGiroscopio;
    public static string DatosUltrasonidos;
}
