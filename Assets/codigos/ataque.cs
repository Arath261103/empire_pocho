using UnityEngine;
using TMPro;

public class AtaquePersonaje : MonoBehaviour
{
    public float radioAtaque = 5f;
    public float velocidadEmbestida = 10f;
    public float velocidadRetroceso = 5f;
    public float daño = 25f;

    public GameObject prefabTextoDaño; // Prefab del texto flotante

    private bool haEmbestido = false;

    void Update()
    {
        // Buscar el objetivo más cercano entre "Barbaro" y "Edificio"
        GameObject objetivo = BuscarObjetivoMasCercano();

        if (objetivo != null)
        {
            float distancia = Vector3.Distance(transform.position, objetivo.transform.position);

            if (distancia <= radioAtaque && !haEmbestido)
            {
                StartCoroutine(EmbestidaConRetroceso(objetivo));
            }
        }
    }

    GameObject BuscarObjetivoMasCercano()
    {
        // Obtiene todos los objetos con tag "Barbaro" y "Edificio"
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Barbaro");
        GameObject[] edificios = GameObject.FindGameObjectsWithTag("Edificio");

        GameObject masCercano = null;
        float menorDistancia = Mathf.Infinity;

        // Recorre los enemigos
        foreach (GameObject obj in enemigos)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < menorDistancia)
            {
                menorDistancia = dist;
                masCercano = obj;
            }
        }

        // Recorre los edificios
        foreach (GameObject obj in edificios)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < menorDistancia)
            {
                menorDistancia = dist;
                masCercano = obj;
            }
        }

        return masCercano;
    }

    System.Collections.IEnumerator EmbestidaConRetroceso(GameObject objetivo)
    {
        haEmbestido = true;
        Vector3 direccion = (objetivo.transform.position - transform.position).normalized;

        // Embestida
        float t = 0f;
        while (t < 0.2f)
        {
            transform.position += direccion * velocidadEmbestida * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }

        // Aplicar daño
        EnemyVida vidaScript = objetivo.GetComponent<EnemyVida>();
        if (vidaScript != null)
        {
            Debug.Log("Daño aplicado: " + daño);
            vidaScript.RecibirDaño(daño);

            // Mostrar el daño como texto flotante sobre el objetivo
            MostrarTextoDaño(daño, objetivo);
        }

        // Retroceso
        Vector3 retroceso = -direccion;
        float r = 0f;
        while (r < 0.2f)
        {
            transform.position += retroceso * velocidadRetroceso * Time.deltaTime;
            r += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        haEmbestido = false;
    }

    void MostrarTextoDaño(float cantidad, GameObject objetivo)
    {
        if (prefabTextoDaño != null)
        {
            Vector3 posicionTexto = objetivo.transform.position;

            // Si el objetivo tiene un collider, usar sus bounds para ubicar el texto en la parte superior
            Collider objetivoCollider = objetivo.GetComponent<Collider>();
            if (objetivoCollider != null)
            {
                posicionTexto = objetivoCollider.bounds.center + Vector3.up * (objetivoCollider.bounds.extents.y + 0.5f);
            }
            else
            {
                // Si no tiene collider, se usa un offset fijo
                posicionTexto += Vector3.up * 2f;
            }

            // Instanciar el prefab del texto en la posición calculada
            GameObject texto = Instantiate(prefabTextoDaño, posicionTexto, Quaternion.identity);

            // Actualizar el texto con TextMesh Pro
            TextMeshPro tm = texto.GetComponent<TextMeshPro>();
            if (tm != null)
            {
                tm.text = "-" + cantidad.ToString();
                tm.color = Color.red;
            }

            texto.transform.SetParent(null);
            Destroy(texto, 2f);
        }
    }
}
