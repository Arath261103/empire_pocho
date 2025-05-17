using UnityEngine;

public class Arquero : PersonajeBase, IVida

{
    public float rangoDeAtaque = 7f;
    public float cadenciaDeDisparo = 1f;
    public GameObject proyectilPrefab;

    private Transform enemigoObjetivo;
    private float tiempoDesdeUltimoDisparo = 0f;
    private Animator anim;


    protected override void Start()
    {
        base.Start(); // Llama al Start de la clase base para inicializar saludActual.
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        BuscarEnemigos();
        Disparar();
    }

    void BuscarEnemigos()
    {
        Collider[] enemigosEnRango = Physics.OverlapSphere(transform.position, rangoDeAtaque);
        Transform enemigoMasCercano = null;
        float distanciaMinima = Mathf.Infinity;
        Vector3 posicionActual = transform.position;

        foreach (Collider enemigo in enemigosEnRango)
        {
            if (enemigo.CompareTag("espadachin_malo") || enemigo.CompareTag("barbaro_malo") ||
              enemigo.CompareTag("arquero_malo") || enemigo.CompareTag("flor_malvada") ||
            enemigo.CompareTag("torre_arquero_malo") || enemigo.CompareTag("torre_espadachin_malo") ||
            enemigo.CompareTag("torre_barbaro_malo"))

            {
                float distanciaAlEnemigo = Vector3.Distance(posicionActual, enemigo.transform.position);
                if (distanciaAlEnemigo < distanciaMinima)
                {
                    enemigoMasCercano = enemigo.transform;
                    distanciaMinima = distanciaAlEnemigo;
                }
            }
        }

        enemigoObjetivo = enemigoMasCercano;
    }


    void Disparar()
    {
        if (enemigoObjetivo != null && Time.time >= tiempoDesdeUltimoDisparo + 1f / cadenciaDeDisparo)
        {

            if (anim != null)
            {
                anim.SetTrigger("attackTrigger");
            }
            GameObject proyectilGO = Instantiate(proyectilPrefab, transform.position, Quaternion.identity);
            Proyectil proyectil = proyectilGO.GetComponent<Proyectil>();

            if (proyectil != null)
            {
                string tipoEnemigo = enemigoObjetivo.gameObject.tag;
                int daño = 0;

                if (tipoEnemigo == "barbaro_malo" || tipoEnemigo == "torre_barbaro_malo")
                {
                    daño = 25;
                }
                else if (tipoEnemigo == "espadachin_malo" || tipoEnemigo == "torre_espadachin_malo")
                {
                    daño = 15;
                }
                else if (tipoEnemigo == "arquero_malo" || tipoEnemigo == "torre_arquero_malo")
                {
                    daño = 20;
                }
                else if (tipoEnemigo == "flor_malvada")
                {
                    daño = 30;
                }


                proyectil.Configurar(enemigoObjetivo, daño);
            }
            tiempoDesdeUltimoDisparo = Time.time;
        }
    }


    // No necesitamos sobreescribir RecibirDaño ni Morir si la lógica base es suficiente.

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
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