using UnityEngine;

public class SpawnerDeMaizEnTodoElTerreno : MonoBehaviour
{
    public GameObject maizPrefab;
    public int cantidadDeMaicesPrincipales = 6;
    public float distanciaEntreMaices = 1.5f;
    public Terrain terreno; // Referencia al Terrain

    void Start()
    {
        // Obtener el tamaño del terreno
        Vector3 size = terreno.terrainData.size;

        for (int i = 0; i < cantidadDeMaicesPrincipales; i++)
        {
            // Elegir posición aleatoria basada en el tamaño real del terreno
            float randomX = Random.Range(0, size.x);
            float randomZ = Random.Range(0, size.z);

            // La posición global
            Vector3 posicionPrincipal = new Vector3(randomX + terreno.transform.position.x, 0, randomZ + terreno.transform.position.z);

            // Ajustar la altura
            posicionPrincipal.y = terreno.SampleHeight(posicionPrincipal) + terreno.transform.position.y;

            // Crear un contenedor para el maíz principal y sus decoraciones
            GameObject contenedorMaiz = new GameObject("ContenedorMaiz_" + i);
            contenedorMaiz.transform.position = posicionPrincipal;

            // Instanciar el maíz principal
            GameObject maizCentral = Instantiate(maizPrefab, posicionPrincipal, Quaternion.identity);
            maizCentral.name = "MaizPrincipal_" + i;
            maizCentral.transform.SetParent(contenedorMaiz.transform);

            // Crear maíces decorativos
            CrearMaicesDecorativos(posicionPrincipal, contenedorMaiz);
        }
    }

    void CrearMaicesDecorativos(Vector3 centro, GameObject contenedorMaiz)
    {
        Vector3[] posiciones = new Vector3[]
        {
            new Vector3(distanciaEntreMaices, 0, 0),
            new Vector3(-distanciaEntreMaices, 0, 0),
            new Vector3(0, 0, distanciaEntreMaices),
            new Vector3(0, 0, -distanciaEntreMaices),
            new Vector3(distanciaEntreMaices, 0, distanciaEntreMaices),
            new Vector3(-distanciaEntreMaices, 0, distanciaEntreMaices),
            new Vector3(distanciaEntreMaices, 0, -distanciaEntreMaices),
            new Vector3(-distanciaEntreMaices, 0, -distanciaEntreMaices)
        };

        foreach (Vector3 offset in posiciones)
        {
            Vector3 posicionDecorativa = centro + offset;

            // Ajustar la altura también para cada maíz decorativo
            posicionDecorativa.y = terreno.SampleHeight(posicionDecorativa) + terreno.transform.position.y;

            GameObject maizDecorativo = Instantiate(maizPrefab, posicionDecorativa, Quaternion.identity);
            maizDecorativo.transform.SetParent(contenedorMaiz.transform); // Establecer al contenedor como su padre
        }
    }
}
