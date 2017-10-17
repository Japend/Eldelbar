using UnityEngine;
using System.Collections;

public class DetectaPuerta : MonoBehaviour
{
    private Camera camera;
    public GameObject target;

    public float speed = .2f;
    public float idleSpeed = 1.5f;

    public float temp;
    public float timeIdle;
    public int direccion = 1;

    void Start()
    {
        direccion = 1;
        temp = timeIdle;
        camera = GetComponentInChildren<Camera>();
        //target = GameObject.FindGameObjectWithTag("Player");       
    }

    void LateUpdate()
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(target.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (onScreen)
        {
            target.GetComponent<Renderer>().material.color = Color.red;
            MirarAlJugador();
        }

        else
        {
            target.GetComponent<Renderer>().material.color = Color.white;
            Idle();
        }
    }


    void Idle()
    {
        if (temp > 0)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * idleSpeed * direccion, Space.World);
            temp -= Time.fixedDeltaTime * 0.5f;
        }
        else
        {
            temp = timeIdle;
            direccion *= -1;
        }
    }

    void MirarAlJugador()
    {
        Vector3 vectorJugador = target.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vectorJugador), speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, target.transform.position);
    }
}








