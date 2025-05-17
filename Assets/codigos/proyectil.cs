using UnityEngine;

public class Proyectil : MonoBehaviour
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

        // Si el proyectil se aleja demasiado sin golpear, destr�yelo para evitar problemas.
        if (Vector3.Distance(transform.position, objetivo.position) > 20f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("espadachin_malo") || other.CompareTag("barbaro_malo") ||
            other.CompareTag("arquero_malo") || other.CompareTag("flor_malvada") ||
            other.CompareTag("torre_arquero_malo") || other.CompareTag("torre_espadachin_malo") ||
            other.CompareTag("torre_barbaro_malo"))
        {
            Debug.Log("Proyectil colision� con: " + other.name);

            if (other.CompareTag("flor_malvada"))
            {
                FlorMalvada flor = other.GetComponent<FlorMalvada>();
                if (flor != null)
                {
                    flor.RecibirDa�o(da�o);
                }
            }
            else
            {
                PersonajeBase personaje = other.GetComponent<PersonajeBase>();
                if (personaje != null)
                {
                    personaje.RecibirDa�o(da�o);
                }
            }

            Destroy(gameObject); // destruye el proyectil despu�s del golpe
        }
    }

}
