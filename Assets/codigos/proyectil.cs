using UnityEngine;

public class Proyectil : MonoBehaviour
{
    private Transform objetivo;
    private int daño;
    public float velocidad = 15f;

    public void Configurar(Transform _objetivo, int _daño)
    {
        objetivo = _objetivo;
        daño = _daño;
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

        // Si el proyectil se aleja demasiado sin golpear, destrúyelo para evitar problemas.
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
            Debug.Log("Proyectil colisionó con: " + other.name);

            if (other.CompareTag("flor_malvada"))
            {
                FlorMalvada flor = other.GetComponent<FlorMalvada>();
                if (flor != null)
                {
                    flor.RecibirDaño(daño);
                }
            }
            else
            {
                PersonajeBase personaje = other.GetComponent<PersonajeBase>();
                if (personaje != null)
                {
                    personaje.RecibirDaño(daño);
                }
            }

            Destroy(gameObject); // destruye el proyectil después del golpe
        }
    }

}
