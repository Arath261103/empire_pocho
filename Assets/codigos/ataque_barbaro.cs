using UnityEngine;
using System.Collections;

public class Barbaro : PersonajeBase,IVida
{
    public float rangoDeAtaque = 5f;
    public float velocidadEmbestida = 10f;
    public float velocidadRetroceso = 5f;
    public float pausaAntesDeRetorno = 0.2f;

    private Transform enemigoObjetivo;
    private Vector3 posicionOriginal;
    private bool enCombate = false;
    private bool estaAtacando = false;
    private Animator anim;


    protected override void Start()
    {
        saludMaxima = 120;
        base.Start();
        posicionOriginal = transform.position;
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (!enCombate)
        {
            BuscarEnemigos();
        }
        else if (enemigoObjetivo != null && !estaAtacando)
        {
            estaAtacando = true;
            posicionOriginal = transform.position; // Guarda la posici�n antes de embestir
            StartCoroutine(EmbestidaConRetroceso());
        }
    }

    void BuscarEnemigos()
    {
        Collider[] enemigosEnRango = Physics.OverlapSphere(transform.position, rangoDeAtaque);

        foreach (Collider enemigo in enemigosEnRango)
        {
            if (enemigo.CompareTag("espadachin_malo") || enemigo.CompareTag("barbaro_malo") ||
    enemigo.CompareTag("arquero_malo") || enemigo.CompareTag("flor_malvada") ||
    enemigo.CompareTag("torre_arquero_malo") || enemigo.CompareTag("torre_espadachin_malo") ||
    enemigo.CompareTag("torre_barbaro_malo"))

            {
                enemigoObjetivo = enemigo.transform;
                enCombate = true;
                break;
            }
        }
    }


    IEnumerator EmbestidaConRetroceso()
    {
        if (enemigoObjetivo == null)
        {
            estaAtacando = false;
            enCombate = false;
            yield break;
        }

        if (anim != null)
        {
            anim.SetTrigger("attackTrigger");
        }


        // Direcci�n hacia el enemigo
        Vector3 direccion = (enemigoObjetivo.position - transform.position).normalized;

        float t = 0f;
        while (t < 0.2f && enemigoObjetivo != null)
        {
            transform.position += direccion * velocidadEmbestida * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }

        if (enemigoObjetivo != null)
        {
            InfligirDa�o(enemigoObjetivo.GetComponent<PersonajeBase>());
        }

        // Espera antes del retroceso
        yield return new WaitForSeconds(pausaAntesDeRetorno);

        // Retroceso a la posici�n original
        Vector3 direccionRetorno = (posicionOriginal - transform.position).normalized;
        float r = 0f;
        while (r < 0.2f)
        {
            transform.position += direccionRetorno * velocidadRetroceso * Time.deltaTime;
            r += Time.deltaTime;
            yield return null;
        }

        transform.position = posicionOriginal;
        enemigoObjetivo = null;
        enCombate = false;
        estaAtacando = false;
    }

    void InfligirDa�o(PersonajeBase enemigo)
    {
        if (enemigo != null)
        {
            string tipoEnemigo = enemigo.gameObject.tag;
            int da�o = 0;

            if (tipoEnemigo == "arquero_malo" || tipoEnemigo == "torre_arquero_malo") da�o = 15;
            else if (tipoEnemigo == "barbaro_malo" || tipoEnemigo == "torre_barbaro_malo") da�o = 20;
            else if (tipoEnemigo == "espadachin_malo" || tipoEnemigo == "torre_espadachin_malo") da�o = 25;

            enemigo.RecibirDa�o(da�o);
        }
        else if (enemigoObjetivo.CompareTag("flor_malvada"))
        {
            FlorMalvada flor = enemigoObjetivo.GetComponent<FlorMalvada>();
            if (flor != null)
            {
                flor.RecibirDa�o(30);
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeAtaque);
    }
    public float GetVida()
    {
        return saludActual;
    }

    public float GetVidaMaxima()
    {
        return saludMaxima;
    }

}
