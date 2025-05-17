using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class spawn_enemigo : MonoBehaviour
{
    public GameObject enemigoBarbaro;
    public GameObject enemigoEspadachin;
    public GameObject enemigoArquero;

    private bool cambiandoModo = false;
    private int estadoModo = 0;



    public TextMeshProUGUI tiempoTexto;
    public TextMeshProUGUI modoTexto;
    private float tiempoSpawnNormal = 6f;
    private float tiempoSpawnDificilMin = 2f;
    private float tiempoSpawnDificilMax = 6f;
    private int contadorPacifico = 0;


    private float tiempoRestante;
    private string modoActual;
    private Coroutine modoCoroutine;

    private bool generandoEnemigos = false;
    private List<Torre_Mala> todasLasTorres = new List<Torre_Mala>();

    private void Start()
    {
        // Buscar todas las torres en escena con el tag "Torre"
        Torre_Mala[] torresEncontradas = GameObject.FindObjectsOfType<Torre_Mala>();
        todasLasTorres = new List<Torre_Mala>(torresEncontradas);

        Debug.Log("Torres encontradas: " + todasLasTorres.Count);


        IniciarModo("Pacífico", 60);//aqui
    }

    private void Update()
    {
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            tiempoTexto.text = "Tiempo: " + Mathf.CeilToInt(tiempoRestante).ToString();
        }

        if (tiempoRestante <= 0 && !cambiandoModo)
        {
            cambiandoModo = true;
            CambiarModo();
        }
    }

    void IniciarModo(string nuevoModo, float duracion)
    {
        modoActual = nuevoModo;
        modoTexto.text = "Modo: " + modoActual;
        tiempoRestante = duracion;

        if (modoCoroutine != null)
            StopCoroutine(modoCoroutine);

        // Aumentar dificultad cada vez que volvemos a modo pacífico (excepto el primero)
        if (modoActual == "Pacífico")
        {
            contadorPacifico++;

            if (contadorPacifico > 1)
            {
                tiempoSpawnNormal = Mathf.Max(0.5f, tiempoSpawnNormal - 1f);
                tiempoSpawnDificilMin = Mathf.Max(0.3f, tiempoSpawnDificilMin - 0.3f);
                tiempoSpawnDificilMax = Mathf.Max(0.5f, tiempoSpawnDificilMax - 0.3f);

                Debug.Log($"[DIFICULTAD ↑] Normal: {tiempoSpawnNormal}s | Difícil: {tiempoSpawnDificilMin}-{tiempoSpawnDificilMax}s");
            }
        }

        if (modoActual == "Normal" || modoActual == "Difícil")
        {
            modoCoroutine = StartCoroutine(GenerarEnemigos());
        }
        cambiandoModo = false;

    }

    Vector3 ObtenerPosicionAleatoriaFueraDelEdificio(Vector3 centro, float radio = 5f)
    {
        Vector3 spawnPos;
        int intentos = 10;

        do
        {
            float angle = Random.Range(0f, 360f);
            float distancia = Random.Range(radio, radio * 1.5f);
            spawnPos = centro + new Vector3(Mathf.Cos(angle) * distancia, 0, Mathf.Sin(angle) * distancia);
            intentos--;
        }
        while (intentos > 0 && Physics.CheckSphere(spawnPos, 0.5f));

        return spawnPos;
    }



    void CambiarModo()
    {
        estadoModo++;

        switch (estadoModo % 4)
        {
            case 1: // después de Pacífico inicial → Normal
                IniciarModo("Normal", 50);
                break;
            case 2: // después de Normal → Pacífico
                IniciarModo("Pacífico", 25);
                break;
            case 3: // después de Pacífico → Difícil
                IniciarModo("Difícil", 60);
                break;
            case 0: // después de Difícil → Pacífico
                IniciarModo("Pacífico", 20);
                break;
        }
    }

    IEnumerator GenerarEnemigos()
    {
        while (true)
        {
            float espera = (modoActual == "Normal")
                ? tiempoSpawnNormal
                : Random.Range(tiempoSpawnDificilMin, tiempoSpawnDificilMax);

            yield return new WaitForSeconds(espera);
            SpawnEnemigoAleatorio();
        }
    }
    void SpawnEnemigoAleatorio()
    {
        if (todasLasTorres.Count == 0)
            return;

        // Mezclamos la lista para no spawnear siempre en el mismo orden
        todasLasTorres.Shuffle();

        // Tomamos la primera torre de la lista (aleatoria por el shuffle)
        Torre_Mala torreSeleccionada = todasLasTorres[0];

        SpawnEnemigoEnTorre(torreSeleccionada);
    }



    void SpawnEnemigoEnTorre(Torre_Mala torre)
    {
        GameObject prefab = null;
        switch (torre.tipo)
        {
            case Torre_Mala.TipoTorre.Barbaro:
                prefab = enemigoBarbaro;
                break;
            case Torre_Mala.TipoTorre.Espadachin:
                prefab = enemigoEspadachin;
                break;
            case Torre_Mala.TipoTorre.Arquero:
                prefab = enemigoArquero;
                break;
        }

        if (prefab != null)
        {
            Vector3 spawnPos = ObtenerPosicionAleatoriaFueraDelEdificio(torre.transform.position, 5f);
            Instantiate(prefab, spawnPos, Quaternion.identity);

        }
    }
}

public static class Extensiones
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}
