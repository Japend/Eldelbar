using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibrador : MonoBehaviour {

    //valor que queda justo por encima del obtenido cuando el dispositivo esta reposado en una superficie plana
    private const float VALOR_MINIMO_ANGULOS = 7.0F;

    //frames durante los que se calculara el valor medio de la fuerza de gravedad
    private const int VALORES_MEDICION_GRAVEDAD = 45;

    //valor medio de la gravedad, se actualizara en cada calibracion
    private static int valorGravedad = 8430;

    private static int contador = 0;

    public static bool Calibrar(float rotX, float rotY, float accelX)
    {
        if (Mathf.Abs(rotX) < VALOR_MINIMO_ANGULOS && Mathf.Abs(rotY) < VALOR_MINIMO_ANGULOS)
        {
            valorGravedad += System.Convert.ToInt32(accelX);
            contador++;

            if (contador >= VALORES_MEDICION_GRAVEDAD)
            {
                valorGravedad = valorGravedad / (contador + 1);
                contador = 0;
                return true;
            }
            return false;
        }

        return false;
    }

    public int GetGravedad()
    {
        return valorGravedad;
    }
}
