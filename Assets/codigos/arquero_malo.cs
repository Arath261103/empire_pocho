using UnityEngine;

public class ArqueroMalo : PersonajeBase, IVida
{
    public float rangoDeAtaque = 7f;
    public float cadenciaDeDisparo = 1f;
    public GameObject proyectilPrefab;

    private Transform objetivoActual;
    private float tiempoDesdeUltimoDisparo = 0f;
    private Animator anim;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        BuscarObjetivos();
        Disparar();
    }

    void BuscarObjetivos()
    {
        Collider[] posiblesObjetivos = Physics.OverlapSphere(transform.position, rangoDeAtaque);
        Transform objetivoMasCercano = null;
        float distanciaMinima = Mathf.Infinity;

        foreach (Collider obj in posiblesObjetivos)
        {
            if (obj.CompareTag("Player") || obj.CompareTag("Edificio1") || obj.CompareTag("Edificio2") || obj.CompareTag("Flor") || obj.CompareTag("Granja"))
            {
                float distancia = Vector3.Distance(transform.position, obj.transform.position);
                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    objetivoMasCercano = obj.transform;
                }
            }
        }

        objetivoActual = objetivoMasCercano;
    }

    void Disparar()
    {
        if (objetivoActual != null && Time.time >= tiempoDesdeUltimoDisparo + 1f / cadenciaDeDisparo)
        {
            if (anim != null)
            {
                anim.SetTrigger("attackTrigger");
            }

            GameObject proyectilGO = Instantiate(proyectilPrefab, transform.position, Quaternion.identity);
            ProyectilMalo proyectil = proyectilGO.GetComponent<ProyectilMalo>();

            if (proyectil != null)
            {
                string tipoTag = objetivoActual.gameObject.tag;
                int daño = 0;

                if (tipoTag == "Player")
                {
                    PersonajeBase jugador = objetivoActual.GetComponent<PersonajeBase>();
                    if (jugador != null)
                    {
                        switch (jugador.tipoPersonaje)
                        {
                            case TipoPersonajeJugador.Arquero:
                                daño = 20;
                                break;
                            case TipoPersonajeJugador.Barbaro:
                                daño = 25;
                                break;
                            case TipoPersonajeJugador.Espadachin:
                                daño = 15;
                                break;
                        }
                    }
                }
                else if (tipoTag == "Edificio1" || tipoTag == "Edificio2" || tipoTag == "Edificio3" || tipoTag == "Granja")
                {
                    daño = 15;
                }
                else if (tipoTag == "Flor")
                {
                    daño = 20;
                }

                proyectil.Configurar(objetivoActual, daño);
            }

            tiempoDesdeUltimoDisparo = Time.time;
        }
    }

    public float GetVida() => saludActual;
    public float GetVidaMaxima() => saludMaxima;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeAtaque);
    }
}
