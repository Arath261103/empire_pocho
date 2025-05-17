using UnityEngine;
using UnityEngine.EventSystems; // Para detectar clics en la UI
using System.Collections.Generic;

public class ClickEdificio : MonoBehaviour
{
    public Material selectedMaterial; // Material de selección
    public GameObject botonUI; // Botón que aparece al seleccionar un edificio
    [System.Serializable]
    public struct PersonajeConfig
    {
        public GameObject prefab;
        public int costo;
    }

    public PersonajeConfig[] personajesConfig;

    public float radioSpawn = 5f; // Radio alrededor del edificio para spawn
    private GameObject edificioSeleccionado = null; // Edificio actualmente seleccionado
    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();
    private ClickGranja clickGranja;
    public int costoSpawn = 10; // Puedes cambiar el valor según tu lógica
    private PersonajeConfig personajeSeleccionado;
    public TMPro.TextMeshProUGUI textoBotonSpawn; // arrástralo en el inspector

    [Header("Límites por tipo")]
    public int maxEspadachines = 30;
    public int maxBarbaros = 20;
    public int maxArqueros = 25;

    private int ContarInstancias(GameObject prefab)
    {
        GameObject[] todos = FindObjectsOfType<GameObject>();
        int contador = 0;

        foreach (GameObject obj in todos)
        {
            if (obj.name.Contains(prefab.name))
            {
                contador++;
            }
        }

        return contador;
    }





    // Para mantener el edificio asociado con su prefab correspondiente
    private GameObject personajePrefab;

    void Start()
    {
        clickGranja = FindFirstObjectByType<ClickGranja>();

        if (botonUI != null)
        {
            botonUI.SetActive(false);
        }
    }

    void Update()
    {
        // Evita detectar clics si el puntero está sobre la UI
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        DetectarClick();
    }

    private void DetectarClick()
    {
        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject objetoClickeado = hit.collider.gameObject;

                if (objetoClickeado.CompareTag("Edificio1") || objetoClickeado.CompareTag("Edificio2") || objetoClickeado.CompareTag("Edificio3"))
                {
                    SeleccionarEdificio(objetoClickeado);
                }
                else
                {
                    DeseleccionarEdificio();
                }
            }
            else
            {
                DeseleccionarEdificio();
            }
        }
    }

    private void SeleccionarEdificio(GameObject nuevoEdificio)
    {
        if (edificioSeleccionado != null)
        {
            RestaurarMaterial(edificioSeleccionado);
        }

        edificioSeleccionado = nuevoEdificio;

        // Guardamos el material original
        if (!originalMaterials.ContainsKey(nuevoEdificio))
        {
            Renderer renderer = nuevoEdificio.GetComponent<Renderer>();
            if (renderer != null)
            {
                originalMaterials[nuevoEdificio] = renderer.material;
                renderer.material = selectedMaterial; // Cambia al material iluminado
            }
        }

        // Asigna el prefab del personaje correspondiente según el edificio seleccionado
        AsignarPersonajePrefab();

        // Mostrar el botón si hay uno asignado
        if (botonUI != null)
        {
            botonUI.SetActive(true);
        }
    }

    private void DeseleccionarEdificio()
    {
        if (edificioSeleccionado != null)
        {
            RestaurarMaterial(edificioSeleccionado);
            edificioSeleccionado = null;
        }

        // Ocultar el botón
        if (botonUI != null)
        {
            botonUI.SetActive(false);
        }
    }

    private void RestaurarMaterial(GameObject obj)
    {
        if (originalMaterials.ContainsKey(obj))
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = originalMaterials[obj]; // Restaurar material original
            }
            originalMaterials.Remove(obj);
        }
    }

    // Método que se conecta al botón para spawnear un personaje
    public void SpawnPersonaje()
    {
        Debug.Log("Intentando spawnear personaje...");

        if (clickGranja == null)
        {
            Debug.LogError("ClickGranja no encontrado.");
            return;
        }

        if (clickGranja.cantidadMaiz < personajeSeleccionado.costo)
        {
            Debug.Log("No tienes suficiente maíz para spawnear un personaje.");
            return;
        }

        // Verifica límite según el tipo de personaje
        int cantidadActual = ContarInstancias(personajeSeleccionado.prefab);

        if ((edificioSeleccionado.CompareTag("Edificio1") && cantidadActual >= maxEspadachines) ||
            (edificioSeleccionado.CompareTag("Edificio3") && cantidadActual >= maxBarbaros) ||
            (edificioSeleccionado.CompareTag("Edificio2") && cantidadActual >= maxArqueros))
        {
            Debug.Log("Límite alcanzado para este tipo de personaje.");
            return;
        }

        // Instanciación permitida
        if (edificioSeleccionado != null && personajeSeleccionado.prefab != null)
        {
            Vector3 spawnPos = ObtenerPosicionAleatoriaFueraDelEdificio(edificioSeleccionado.transform.position);
            Instantiate(personajeSeleccionado.prefab, spawnPos, Quaternion.identity);

            clickGranja.cantidadMaiz -= personajeSeleccionado.costo;
            clickGranja.SendMessage("ActualizarTextoMaiz");

            Debug.Log("Personaje spawneado en: " + spawnPos);
        }
        else
        {
            Debug.LogError("No hay edificio seleccionado o falta el prefab del personaje.");
        }
    }



    // Obtiene una posición aleatoria fuera del edificio dentro del radio especificado
    private Vector3 ObtenerPosicionAleatoriaFueraDelEdificio(Vector3 centro)
    {
        Vector3 spawnPos;
        int intentos = 10;

        do
        {
            float angle = Random.Range(0f, 360f);
            float distancia = Random.Range(radioSpawn, radioSpawn * 1.5f);
            spawnPos = centro + new Vector3(Mathf.Cos(angle) * distancia, 0, Mathf.Sin(angle) * distancia);
            intentos--;
        }
        while (intentos > 0 && Physics.CheckSphere(spawnPos, 0.5f)); // Evita colisiones

        return spawnPos;
    }

    // Asigna el prefab del personaje correspondiente según el tag del edificio
    private void AsignarPersonajePrefab()
    {
        if (edificioSeleccionado.CompareTag("Edificio1"))
        {
            personajeSeleccionado = personajesConfig[0];
        }
        else if (edificioSeleccionado.CompareTag("Edificio2"))
        {
            personajeSeleccionado = personajesConfig[1];
        }
        else if (edificioSeleccionado.CompareTag("Edificio3"))
        {
            personajeSeleccionado = personajesConfig[2];
        }

        // Actualiza el texto del botón con el costo
        if (textoBotonSpawn != null)
        {
            textoBotonSpawn.text = $"Spawn: {personajeSeleccionado.costo}";
        }
    }

}
