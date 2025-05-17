using UnityEngine;

public class ProyectilMalo : MonoBehaviour
{
    private Transform objetivo;
    private int da�o;
    public float velocidad = 15f;

    public void Configurar(Transform _objetivo, int _da�o)
    {
        objetivo = _objetivo;
        da�o = _da�o;
    }

    void Update()
    {
        if (objetivo == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direccion = (objetivo.position - transform.position).normalized;
        transform.position += direccion * velocidad * Time.deltaTime;

        if (Vector3.Distance(transform.position, objetivo.position) > 20f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Edificio1") || other.CompareTag("Edificio2") || other.CompareTag("Edificio3") || other.CompareTag("Flor") || other.CompareTag("Granja"))
        {
            Debug.Log("ProyectilMalo colision� con: " + other.name);

            if (other.CompareTag("Flor"))
            {
                Flor flor = other.GetComponent<Flor>();
                if (flor != null)
                {
                    flor.RecibirDa�o(da�o);
                }
            }
            else if (other.CompareTag("Edificio1") || other.CompareTag("Edificio2") || other.CompareTag("Edificio3") || other.CompareTag("Granja"))
            {
                Torre torre = other.GetComponent<Torre>();
                if (torre != null)
                {
                    torre.RecibirDa�o(da�o);
                }
            }
            else if (other.CompareTag("Player"))
            {
                PersonajeBase jugador = other.GetComponent<PersonajeBase>();
                if (jugador != null)
                {
                    jugador.RecibirDa�o(da�o);
                }
            }

            Destroy(gameObject);
        }
    }
}
