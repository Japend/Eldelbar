using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaMuerte : MonoBehaviour {

    public bool fin;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "jarra")
        {
            fin = true;
            GlobalData.EstadoJuego = GlobalData.FIN_DEL_JUEGO;
        }
    }
}
