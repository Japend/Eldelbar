#define DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorObstaculos : MonoBehaviour {

	private class Obstaculo
	{
		GameObject obstaculo;
		public Vector3 velocidad;
		public bool usandose;

		public Obstaculo(GameObject obs)
		{
			obstaculo = obs;
			velocidad = new Vector3();
			usandose = false;
		}

		public GameObject GetObject()
		{
			return obstaculo;
		}

        public void SetUsandose(bool value)
        {
            usandose = value;
        }
	}
	/* ------------------------------------------------------
	 * ---------------TIPOS OBSTACULOS-----------------------
	 * ------------------------------------------------------
	 * TIPO 1: Obstaculos normales: Tamano medio, velocidad media.
	 * TIPO 2: Obstaculos rapidos: Tamano pequeno, velocidad rapida
	 * TIPO 3: Obstaculos grandes: Gran tamano, vel lenta
	 * TIPO 4: Obstaculos especiales: El jugador solo puede salvarse interactuando con los ultrasonidos. Ocupan todo el escenario
	 * menos zonas seguras
	 * */

	private const int X_MIN = 0;
	private const int X_MAX= 1;
	private const int Y_MIN= 2;
	private const int Y_MAX= 3;

	private const int NORMALES = 0;
	private const int RAPIDOS = 1;
	private const int GRANDES = 2;
	private const int ESPECIALES = 3;

	private const float TIEMPO_MINIMO_ENTRE_OBSTACULOS = 2.0F;
	private const float TIEMPO_MAXIMO_ENTRE_OBSTACULOS = 3.0F;
	private const float Z_INICIAL = 14;
    private const float Z_FINAL = -10;


	public GameObject[][] obstaculosBase;
    public Vector3 velocidadBase; //velocidad que usaran los obstaculos normales *1, los rapidos *2 y los grnades *0.5

	List<Obstaculo> obstaculosActivos, obstaculosParaBorrar;
	Obstaculo[][][] piscinaObstaculos;
	float[] valoresFrontera;
	float tiempoRestanteAparicionNormal, tiempoRestanteAparicionTipos23;


	void Awake () {

        /*se obtiene las coordenadas de las esquinas para definir el espacio en el que pueden aparecer los obstaculos*/
		Vector3 aux;
        valoresFrontera = new float[4];
		aux = GameObject.Find ("NE").transform.position;
		valoresFrontera [X_MAX] = aux.x;
		valoresFrontera [Y_MAX] = aux.y;

        aux = GameObject.Find("SW").transform.position;
		valoresFrontera [X_MIN] = aux.x;
		valoresFrontera [Y_MAX] = aux.y;

		//cargando obstaculos
		cargaObstaculos();

		//generando piscina de obstaculos
		generaPiscinaObstaculos();
		tiempoRestanteAparicionNormal = TIEMPO_MAXIMO_ENTRE_OBSTACULOS;
		tiempoRestanteAparicionTipos23 = TIEMPO_MAXIMO_ENTRE_OBSTACULOS * 2;

        //inicilizando variables
        obstaculosActivos = new List<Obstaculo>();
        obstaculosParaBorrar = new List<Obstaculo>();
	}
	

    //control de temporizadores para aparicion de obstaculos
	void Update () {
		tiempoRestanteAparicionNormal -= Time.deltaTime;
		if (tiempoRestanteAparicionNormal <= 0) {
			generarObstaculo (NORMALES);
            tiempoRestanteAparicionNormal = Random.Range(TIEMPO_MAXIMO_ENTRE_OBSTACULOS, TIEMPO_MINIMO_ENTRE_OBSTACULOS);
		}
	}


    //movimiento de los obstaculos activos
    void FixedUpdate()
    {
        //se mueven todos los obstaculos activos
        foreach (Obstaculo obs in obstaculosActivos)
        {
            obs.GetObject().transform.position += obs.velocidad * Time.fixedDeltaTime;
            if (obs.GetObject().transform.position.z < Z_FINAL)
                obstaculosParaBorrar.Add(obs);
        }

        //se borran aquellos que han llegado al limite del mapa
        for (int i = 0; i < obstaculosParaBorrar.Count; i++)
        {
            obstaculosActivos.Remove(obstaculosParaBorrar[i]);
            obstaculosParaBorrar[i].usandose = false;
            obstaculosParaBorrar[i].GetObject().SetActive(false);
        }
        obstaculosParaBorrar.Clear();
    }

	private void cargaObstaculos()
	{
        /*lee los obstaculos en la escenar*/
		GameObject obstaculoAux;
		obstaculosBase = new GameObject[4][];
		piscinaObstaculos = new Obstaculo[4][][];

        //los obstaculos se encuetran en gameobjects llamados "ObstaculosTipox" donde x es el tipo
		for (int i = 0; i < 4; i++) {
			obstaculoAux = GameObject.Find("ObstaculosTipo" + i);
			obstaculosBase [i] = new GameObject[obstaculoAux.transform.childCount];

            for (int j = 0; j < obstaculoAux.transform.childCount; j++)
                obstaculosBase[i][j] = obstaculoAux.transform.GetChild(j).gameObject;
		}
	}

	private void generaPiscinaObstaculos()
	{
		piscinaObstaculos = new Obstaculo[4][][];
		//se genera una piscina de obstaculos para evitar instanciar o eliminar objetos
		for (int i = 0; i < obstaculosBase.Length; i++) {
			piscinaObstaculos [i] = new Obstaculo[obstaculosBase [i].Length][];
			for(int j = 0; j < obstaculosBase[i].Length; j++){
                piscinaObstaculos[i][j] = new Obstaculo[5];
				for (int k = 0; k < 5; k++) {
					piscinaObstaculos [i] [j] [k] = new Obstaculo (GameObject.Instantiate (obstaculosBase [i] [j]));
                    piscinaObstaculos[i][j][k].GetObject().SetActive(false);
				}
			}
		}

        #if DEBUG
        for (int i = 0; i < piscinaObstaculos.Length; i++)
        {
            for (int j = 0; j < piscinaObstaculos[i].Length; j++)
            {
                for (int k = 0; k < piscinaObstaculos[i][j].Length; k++)
                {
                    Debug.Log(System.Convert.ToInt32(i) + "/" + System.Convert.ToInt32(j) + "/" + System.Convert.ToInt32(k) + "/" + piscinaObstaculos[i][j][k].GetObject().name);
                }
            }
        }
#endif
	}

    private void generarObstaculo(int i)
	{
		//recorre la piscina correspondiente al obstaculo
		bool ok = false;
		int aux = 0;
		Obstaculo seleccion;

		while (!ok) {
			aux = Random.Range(0, piscinaObstaculos[i].Length);
			for (int j = 0; j < 5; j++) {
				//si hay alguno sin usar, lo elige

#if DEBUG
                Debug.Log(System.Convert.ToInt32(i) + "/" + System.Convert.ToInt32(aux));
#endif

				if (!piscinaObstaculos [i] [aux] [j].usandose) {
                    
                    prepararObstaculo(piscinaObstaculos[i][aux][j], aux);

                    //se anade a la lista de obstaculos activos
                    obstaculosActivos.Add(piscinaObstaculos[i][aux][j]);
					ok = true;
					break;
				}
			}
				//si no, prueba con otro obstaculo, si no quedan no se instancia ninguno
			if (aux < piscinaObstaculos [i].Length - 1 )
					aux++;
			else
					return;
		}


	}

    private void prepararObstaculo(Obstaculo obs, int tipo)
    {
        obs.GetObject().SetActive(true);
        obs.GetObject().transform.position = new Vector3(Random.Range(valoresFrontera[X_MIN], valoresFrontera[X_MAX]),
            Random.Range(valoresFrontera[Y_MIN], valoresFrontera[Y_MAX]), Z_INICIAL);
        obs.usandose = true;

#if DEBUG
        Debug.Log(obs.GetObject().transform.position);
#endif
        switch (tipo)
        {
            case NORMALES:
                obs.velocidad.z = velocidadBase.z * Random.Range(0.8f, 1.1f);
                break;

            case RAPIDOS:
                obs.velocidad.z = velocidadBase.z * Random.Range(1.8f, 2.2f);
                break;

            case GRANDES:
                obs.velocidad.z = velocidadBase.z * Random.Range(0.3f, 0.6f);
                break;

            case ESPECIALES:
                //pendiente de decision
                break;
        }
    }

}
