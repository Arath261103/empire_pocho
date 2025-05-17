using UnityEngine;
using UnityEngine.AI;

public class EnemigoSeguidor : MonoBehaviour
{
    public float radioDeteccion = 10f; // Radio dentro del cual el enemigo detecta al jugador
    private NavMeshAgent navAgent; // Componente NavMeshAgent del enemigo
    private Transform jugadorMasCercano; // Jugador más cercano

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        jugadorMasCercano = EncontrarJugadorMasCercano();

        if (jugadorMasCercano != null)
        {
            float distancia = Vector3.Distance(transform.position, jugadorMasCercano.position);

            if (distancia <= radioDeteccion)
            {
                navAgent.SetDestination(jugadorMasCercano.position);
            }
            else
            {
                navAgent.ResetPath();
            }
        }
    }

    private Transform EncontrarJugadorMasCercano()
    {
        GameObject[] jugadores = GameObject.FindGameObjectsWithTag("Player");
        Transform masCercano = null;
        float distanciaMinima = Mathf.Infinity;

        foreach (GameObject jugador in jugadores)
        {
            float distancia = Vector3.Distance(transform.position, jugador.transform.position);
            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                masCercano = jugador.transform;
            }
        }

        return masCercano;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}
