using UnityEngine;
using TMPro;

public class ContadorPorPrefab : MonoBehaviour
{
    [Header("Prefabs de personajes")]
    public GameObject prefab1; // Espadachín
    public GameObject prefab2; // Bárbaro
    public GameObject prefab3; // Arquero

    [Header("Límites máximos")]
    public int maxEspadachines = 30;
    public int maxBarbaros = 20;
    public int maxArqueros = 25;

    [Header("Contadores para cada tipo")]
    public TextMeshProUGUI contadorPrefab1;
    public TextMeshProUGUI contadorPrefab2;
    public TextMeshProUGUI contadorPrefab3;

    void Update()
    {
        int count1 = ContarInstancias(prefab1);
        int count2 = ContarInstancias(prefab2);
        int count3 = ContarInstancias(prefab3);

        contadorPrefab1.text = $"Espadachines: {count1}/{maxEspadachines}";
        contadorPrefab2.text = $"Barbaros: {count2}/{maxBarbaros}";
        contadorPrefab3.text = $"Arqueros: {count3}/{maxArqueros}";
    }

    int ContarInstancias(GameObject prefab)
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
}
