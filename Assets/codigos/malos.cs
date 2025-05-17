using UnityEngine;
using UnityEngine.AI;

public class PerseguirObjetivo : MonoBehaviour
{
    private Transform objetivo; // Se asignar� autom�ticamente si est� vac�o
    private NavMeshAgent agente;

    public float radioDeteccion = 3f; // Radio para frenar si hay obst�culo cercano

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        // Buscar autom�ticamente el objeto con tag "Flor" si no fue asignado
        if (objetivo == null)
        {
            GameObject flor = GameObject.FindGameObjectWithTag("Flor");
            if (flor != null)
            {
                objetivo = flor.transform;
            }
            else
            {
                Debug.LogWarning("No se encontr� un objeto con el tag 'Flor' en la escena.");
            }
        }
    }

    void Update()
    {
        if (objetivo == null) return;

        if (HayObstaculoCercano())
        {
            agente.isStopped = true;
        }
        else
        {
            agente.isStopped = false;
            agente.SetDestination(objetivo.position);
        }
    }

    bool HayObstaculoCercano()
    {
        Collider[] colisiones = Physics.OverlapSphere(transform.position, radioDeteccion);

        foreach (var col in colisiones)
        {
            if (col.CompareTag("Player") ||
                col.CompareTag("Edificio1") ||
                col.CompareTag("Edificio2") ||
                col.CompareTag("Edificio3"))
            {
                return true;
            }
        }

        return false;
    }
}
