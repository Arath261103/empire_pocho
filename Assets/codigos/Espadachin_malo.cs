using UnityEngine;
using System.Collections;

public class EspadachinMalo : PersonajeBase,IVida
{
    public float rangoDeAtaque = 5f;
    public float velocidadEmbestida = 10f;
    public float velocidadRetroceso = 5f;
    public float pausaAntesDeRetorno = 0.2f;

    private Transform objetivoActual;
    private Vector3 posicionOriginal;
    private bool enCombate = false;
    private bool estaAtacando = false;

    protected override void Start()
    {
        tipoPersonaje = TipoPersonajeJugador.Espadachin;
        saludMaxima = 100;
        base.Start();
        posicionOriginal = transform.position;
    }

    void Update()
    {
        if (!enCombate)
        {
            BuscarObjetivos();
        }
        else if (objetivoActual != null && !estaAtacando)
        {
            estaAtacando = true;
            posicionOriginal = transform.position;
            StartCoroutine(EmbestidaConRetroceso());
        }
    }

    void BuscarObjetivos()
    {
        Collider[] posiblesObjetivos = Physics.OverlapSphere(transform.position, rangoDeAtaque);

        foreach (Collider objetivo in posiblesObjetivos)
        {
            if (objetivo.CompareTag("Player") ||
                objetivo.CompareTag("Edificio1") ||
                objetivo.CompareTag("Edificio2") ||
                objetivo.CompareTag("Edificio3") ||
                objetivo.CompareTag("Granja") ||
                objetivo.CompareTag("Flor"))
            {
                objetivoActual = objetivo.transform;
                enCombate = true;
                break;
            }
        }
    }

    IEnumerator EmbestidaConRetroceso()
    {
        if (objetivoActual == null)
        {
            estaAtacando = false;
            enCombate = false;
            yield break;
        }

        Vector3 direccion = (objetivoActual.position - transform.position).normalized;

        float t = 0f;
        while (t < 0.2f && objetivoActual != null)
        {
            transform.position += direccion * velocidadEmbestida * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }

        if (objetivoActual != null)
        {
            if (objetivoActual.CompareTag("Player"))
            {
                InfligirDaño(objetivoActual.GetComponent<PersonajeBase>());
            }
            else if (objetivoActual.CompareTag("Edificio1") ||
                     objetivoActual.CompareTag("Edificio2") ||
                     objetivoActual.CompareTag("Edificio3") ||
                     objetivoActual.CompareTag("Granja"))
            {
                Torre torre = objetivoActual.GetComponent<Torre>();
                if (torre != null)
                {
                    torre.RecibirDaño(15);
                }
            }
            else if (objetivoActual.CompareTag("Flor"))
            {
                Flor flor = objetivoActual.GetComponent<Flor>();
                if (flor != null)
                {
                    flor.RecibirDaño(10);
                }
            }
        }

        yield return new WaitForSeconds(pausaAntesDeRetorno);

        Vector3 direccionRetorno = (posicionOriginal - transform.position).normalized;
        float r = 0f;
        while (r < 0.2f)
        {
            transform.position += direccionRetorno * velocidadRetroceso * Time.deltaTime;
            r += Time.deltaTime;
            yield return null;
        }

        transform.position = posicionOriginal;
        objetivoActual = null;
        enCombate = false;
        estaAtacando = false;
    }

    void InfligirDaño(PersonajeBase personaje)
    {
        if (personaje != null)
        {
            int daño = 0;

            switch (personaje.tipoPersonaje)
            {
                case TipoPersonajeJugador.Espadachin:
                    daño = 20;
                    break;
                case TipoPersonajeJugador.Barbaro:
                    daño = 15;
                    break;
                case TipoPersonajeJugador.Arquero:
                    daño = 25;
                    break;
            }

            personaje.RecibirDaño(daño);
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
